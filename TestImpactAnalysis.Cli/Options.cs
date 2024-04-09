using CommandLine;
using TestImpactAnalysis.Coverage.Impl;
using TestImpactAnalysis.Database;

namespace TestImpactAnalysis.Cli;

public class Options
{
    [Option('d', "testsdll", Required = true, HelpText = "Path to tests dll")]
    public string PathToTestsDll { get; set; }
    
    [Option('t', "testsproj", Required = true, HelpText = "Path to directory with tests csproj file")]
    public string PathToTestsProject { get; set; }

    [Option('g', "git", Required = true, HelpText = "Path to directory with .git folder")]
    public string PathToDirectoryWithGit { get; set; }
    
    [Option('b', "commit1", Required = true, HelpText = "First commit hash")]
    public string Commit1Hash { get; set; }

    [Option('a', "commit2", Required = true, HelpText = "Second commit hash")]
    public string Commit2Hash { get; set; }
    
    [Option('c', "connection", Required = true, HelpText = "Connection string to database")]
    public string DbConnection { get; set; }

    [Option('s', "dbtype", Required = true, HelpText = "Type of database")]
    public DatabaseType DbType { get; set; }
    
    [Option('v', "verbosity", Required = true, HelpText = "Logging detail level")]
    public CmdTestRunner.OutputDetalization Verbosity { get; set; }
}