# Mutagen.Bethesda.Analyzers

Currently in the research stage, this is a place to gather and compile information about known errors within Bethesda games that cause crashes, oddities, or other issues.

These will be used to power a tool that is able to quickly analyze a mod/load order and give a succinct listing of all the things that might cause CTDs, other issues, or perhaps look odd or atypical about a mod.

Other tools exist that can do aspects of this, but the focus for this eventual library would be offering all the interesting analysis results in one place that can be accessed and interacted with programatically by being exposed as a reusable library, and then eventually perhaps down the road a GUI/CLI that gives a typical user all the details in one place without needing to hunt down so many disparate tools/subplugins to run.

Another aspect that this library will strive for is live responsiveness.  It should have the ability to watch a folder/file and analyze + report issues in real time, short circuiting areas that did not change, and showing errors that show up or are removed in the mod/load order as the user modifies things on-disk.  Thus, they could hopefully leave the tool open and see the issues, while solving them elsewhere and seeing the errors disappear as they make the changes.
