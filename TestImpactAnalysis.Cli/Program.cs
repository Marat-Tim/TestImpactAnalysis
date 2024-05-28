using CommandLine;
using Microsoft.Extensions.Logging;
using TestImpactAnalysis.Default;
using Options = TestImpactAnalysis.Cli.Options;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(options =>
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole()
                .SetMinimumLevel(options.LogLevel);
        });
        
        var tests = new TestsThatDependsOnChanges(
            options.PathToTestsDll,
            options.PathToTestsProject,
            options.PathToDirectoryWithGit,
            options.Commit1Hash,
            options.Commit2Hash,
            options.DbConnection,
            options.DbType)
        {
            Logger = loggerFactory.CreateLogger<Program>()
        };
        foreach (var test in tests)
        {
            Console.WriteLine(test);
        }
    });