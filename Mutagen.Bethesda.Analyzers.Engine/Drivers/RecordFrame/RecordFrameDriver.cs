using System.Buffers;
using Mutagen.Bethesda.Analyzers.SDK.Analyzers;
using Mutagen.Bethesda.Environments.DI;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Analysis;
using Mutagen.Bethesda.Plugins.Binary.Headers;
using Mutagen.Bethesda.Plugins.Binary.Parameters;
using Mutagen.Bethesda.Plugins.Binary.Streams;
using Mutagen.Bethesda.Plugins.Meta;
using Noggog;
using Noggog.WorkEngine;

namespace Mutagen.Bethesda.Analyzers.Drivers.RecordFrame;

public class RecordFrameDriver : IIsolatedDriver, IContextualDriver
{
    private readonly IWorkDropoff _dropoff;
    private readonly GameConstants _constants;
    private readonly IGameReleaseContext _gameReleaseContext;
    private readonly IDataDirectoryProvider _dataDataDirectoryProvider;

    private class Drivers
    {
        public IIsolatedRecordFrameAnalyzerDriver[] Isolated = [];
        public IContextualRecordFrameAnalyzerDriver[] Contextual = [];
    }

    private readonly Dictionary<RecordType, Drivers> _mapping = new();
    public bool Applicable => _mapping.Count > 0;

    public IEnumerable<IAnalyzer> Analyzers => _mapping.Values.SelectMany(x =>
    {
        return x.Contextual.SelectMany(x => x.Analyzers)
            .Concat(x.Isolated.SelectMany(x => x.Analyzers));
    });

    public RecordFrameDriver(
        IGameReleaseContext gameReleaseContext,
        IDataDirectoryProvider dataDataDirectoryProvider,
        IIsolatedRecordFrameAnalyzerDriver[] isolatedDrivers,
        IContextualRecordFrameAnalyzerDriver[] contextualDrivers,
        IWorkDropoff dropoff)
    {
        _gameReleaseContext = gameReleaseContext;
        _dataDataDirectoryProvider = dataDataDirectoryProvider;
        _dropoff = dropoff;
        _constants = GameConstants.Get(_gameReleaseContext.Release);
        foreach (var drivers in isolatedDrivers
                     .Where(x => x.Applicable)
                     .GroupBy(x => x.TargetType))
        {
            _mapping.GetOrAdd(drivers.Key).Isolated = drivers.ToArray();
        }
        foreach (var drivers in contextualDrivers
                     .Where(x => x.Applicable)
                     .GroupBy(x => x.TargetType))
        {
            _mapping.GetOrAdd(drivers.Key).Contextual = drivers.ToArray();
        }
    }

    public async Task Drive(ContextualDriverParams driverParams)
    {
        foreach (var listing in driverParams.LoadOrder.ListedOrder)
        {
            if (listing.Mod is null) continue;

            var modPath = Path.Combine(_dataDataDirectoryProvider.Path, listing.ModKey.FileName);

            var parsingMeta = ParsingMeta.Factory(new BinaryReadParameters(), _gameReleaseContext.Release, modPath);
            using var stream = new MutagenBinaryReadStream(modPath, parsingMeta);
            var locs = RecordLocator.GetLocations(stream);

            foreach (var recordLocationMarker in locs.ListedRecords)
            {
                if (!_mapping.TryGetValue(recordLocationMarker.Value.Record, out var analyzerBundles)) continue;
                if (analyzerBundles.Isolated.Length == 0 && analyzerBundles.Contextual.Length == 0) continue;

                stream.Position = recordLocationMarker.Value.Location.Min;

                var width = checked((int)recordLocationMarker.Value.Location.Width);

                var buf = ArrayPool<byte>.Shared.Rent(width);

                var amountRead = stream.Read(buf.AsSpan().Slice(0, width));
                if (amountRead != width)
                {
                    throw new DataMisalignedException();
                }

                var frame = new MajorRecordFrame(_constants, buf);

                if (analyzerBundles.Isolated.Length > 0)
                {
                    var isolatedParams = new IsolatedDriverParams(
                        listing.Mod.ToUntypedImmutableLinkCache(),
                        driverParams.ReportDropbox,
                        listing.Mod,
                        modPath);

                    await Task.WhenAll(analyzerBundles.Isolated.Select(analyzer =>
                    {
                        return _dropoff.EnqueueAndWait(() =>
                        {
                            analyzer.Drive(isolatedParams, frame);
                        });
                    }));
                }

                await Task.WhenAll(analyzerBundles.Contextual.Select(analyzer =>
                {
                    return _dropoff.EnqueueAndWait(() =>
                    {
                        analyzer.Drive(driverParams, frame);
                    });
                }));

                await _dropoff.EnqueueAndWait(() =>
                {
                    ArrayPool<byte>.Shared.Return(buf);
                });
            }
        }
    }

    public async Task Drive(IsolatedDriverParams driverParams)
    {
        var parsingMeta = ParsingMeta.Factory(new BinaryReadParameters(), _gameReleaseContext.Release, driverParams.TargetModPath);
        using var stream = new MutagenBinaryReadStream(driverParams.TargetModPath, parsingMeta);
        var locs = RecordLocator.GetLocations(stream);

        foreach (var recordLocationMarker in locs.ListedRecords)
        {
            if (!_mapping.TryGetValue(recordLocationMarker.Value.Record, out var analyzerBundles)
                || analyzerBundles.Isolated.Length == 0) continue;

            stream.Position = recordLocationMarker.Value.Location.Min;

            var width = checked((int)recordLocationMarker.Value.Location.Width);

            var buf = ArrayPool<byte>.Shared.Rent(width);

            var amountRead = stream.Read(buf.AsSpan().Slice(0, width));
            if (amountRead != width)
            {
                throw new DataMisalignedException();
            }

            var frame = new MajorRecordFrame(GameConstants.Get(_gameReleaseContext.Release), buf);

            await Task.WhenAll(analyzerBundles.Isolated.Select(analyzer =>
            {
                return _dropoff.EnqueueAndWait(() =>
                {
                    analyzer.Drive(driverParams, frame);
                });
            }));

            await _dropoff.EnqueueAndWait(() =>
            {
                ArrayPool<byte>.Shared.Return(buf);
            });
        }
    }
}
