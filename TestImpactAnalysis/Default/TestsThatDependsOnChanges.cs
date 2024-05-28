using System.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TestImpactAnalysis.Coverage;
using TestImpactAnalysis.Coverage.Impl;
using TestImpactAnalysis.Database;
using TestImpactAnalysis.ProjectChanges;
using TestImpactAnalysis.ProjectChanges.Impl;
using TestImpactAnalysis.Tests.Impl;

namespace TestImpactAnalysis.Default;

public class TestsThatDependsOnChanges : IEnumerable<string>
{
    private readonly string _pathToTestsDll;
    
    private readonly string _pathToTestsProject;
    
    private readonly string _pathToDirectoryWithGit;
    
    private readonly string _commit1Hash;
    
    private readonly string _commit2Hash;
    
    private readonly string _dbConnection;
    
    private readonly DatabaseType _databaseType;

    public ILogger Logger { init; private get; } = NullLogger.Instance;

    public TestsThatDependsOnChanges(string pathToTestsDll, 
        string pathToTestsProject, 
        string pathToDirectoryWithGit, 
        string commit1Hash, string commit2Hash,
        string dbConnection, DatabaseType databaseType)
    {
        _pathToTestsDll = pathToTestsDll;
        _pathToTestsProject = pathToTestsProject;
        _pathToDirectoryWithGit = pathToDirectoryWithGit;
        _commit1Hash = commit1Hash;
        _commit2Hash = commit2Hash;
        _dbConnection = dbConnection;
        _databaseType = databaseType;
    }
    
    public IEnumerator<string> GetEnumerator()
    {
        IEnumerable<string> tests = new XunitTestList(_pathToTestsDll, Logger);

        string dbName = _databaseType == DatabaseType.SQLite ? "sqlite" : "postgresql";
        Logger.LogDebug($"Using {dbName} with connection string = {_dbConnection}");
        
        using ICoverageRepository coverageRepository = new LoggingRepositoryDecorator(
            new SqlCoverageRepository(_dbConnection, _databaseType), Logger);
        

        ITestRunner testRunner = new CmdTestRunner(_pathToTestsProject, Logger);

        ICoverageExtractor coverageExtractor = new FileCoverageExtractorFromJson(_pathToDirectoryWithGit);

        ICoverageInfo coverageInfo = new CoverageInfo(coverageRepository, testRunner, coverageExtractor, Logger);

        IChanges changes = new Changes(
            new GitChangedFiles(_pathToDirectoryWithGit, _commit1Hash, _commit2Hash, Logger));

        List<string> testsThatDependsOnChanges =
            new TestImpactAnalysis.TestsThatDependsOnChanges(tests, coverageInfo, changes).ToList();

        ICoverageRecalculator coverageRecalculator =
            new CoverageRecalculator(coverageRepository, testRunner, coverageExtractor, Logger);
        
        coverageRecalculator.Recalculate(testsThatDependsOnChanges);

        return testsThatDependsOnChanges.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}