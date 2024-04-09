using CommandLine;
using TestImpactAnalysis.Cli;
using TestImpactAnalysis.Default;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(options =>
    {
        var tests = new TestsThatDependsOnChanges(
            options.PathToTestsDll,
            options.PathToTestsProject,
            options.PathToDirectoryWithGit,
            options.Commit1Hash,
            options.Commit2Hash,
            options.DbConnection,
            options.DbType,
            options.Verbosity);
        foreach (var test in tests)
        {
            Console.WriteLine(test);
        }
    });