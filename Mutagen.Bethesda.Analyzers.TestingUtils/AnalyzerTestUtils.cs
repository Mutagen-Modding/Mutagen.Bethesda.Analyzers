using System.Linq;
using Mutagen.Bethesda.Analyzers.SDK.Errors;
using Mutagen.Bethesda.Analyzers.SDK.Results;
using Xunit;

namespace Mutagen.Bethesda.Analyzers.TestingUtils
{
    public static class AnalyzerTestUtils
    {
        public static void HasError<TError>(IAnalyzerResult<TError> result, ErrorDefinition errorDefinition, int count = -1)
            where TError : IError
        {
            if (count == -1)
            {
                Assert.True(result.Errors.Any(x => x.FormattedErrorDefinition.ErrorDefinition.Equals(errorDefinition)));
            }
            else
            {
                var errors = result.Errors
                    .Where(x => x.FormattedErrorDefinition.ErrorDefinition.Equals(errorDefinition))
                    .ToList();

                Assert.Equal(count, errors.Count);
            }
        }
    }
}
