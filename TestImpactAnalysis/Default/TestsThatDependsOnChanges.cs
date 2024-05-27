using System.Collections;
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

    private readonly CmdTestRunner.OutputDetalization _writeDebug;

    public TestsThatDependsOnChanges(string pathToTestsDll, 
        string pathToTestsProject, 
        string pathToDirectoryWithGit, 
        string commit1Hash, string commit2Hash,
        string dbConnection, DatabaseType databaseType,
        CmdTestRunner.OutputDetalization writeDebug = CmdTestRunner.OutputDetalization.None)
    {
        _pathToTestsDll = pathToTestsDll;
        _pathToTestsProject = pathToTestsProject;
        _pathToDirectoryWithGit = pathToDirectoryWithGit;
        _commit1Hash = commit1Hash;
        _commit2Hash = commit2Hash;
        _dbConnection = dbConnection;
        _databaseType = databaseType;
        _writeDebug = writeDebug;
    }
    
    public IEnumerator<string> GetEnumerator()
    {
        IEnumerable<string> tests = new XunitTestList(_pathToTestsDll);

        ICoverageRepository coverageRepository;
        switch (_databaseType)
        {
            case DatabaseType.SQLite:
                coverageRepository = new SqliteCoverageRepository(_dbConnection);
                break;
            case DatabaseType.PostgreSQL:
                coverageRepository = new PostgresqlCoverageRepository(_dbConnection);
                break;
            default:
                throw new NotImplementedException();
        }

        ITestRunner testRunner = new CmdTestRunner(_pathToTestsProject) { WriteOutput = _writeDebug };

        ICoverageExtractor coverageExtractor = new FileCoverageExtractorFromJson(_pathToDirectoryWithGit);

        ICoverageInfo coverageInfo = new CoverageInfo(coverageRepository, testRunner, coverageExtractor);

        IChanges changes = new Changes(
            new GitChangedFiles(_pathToDirectoryWithGit, _commit1Hash, _commit2Hash));

        List<string> testsThatDependsOnChanges =
            new TestImpactAnalysis.TestsThatDependsOnChanges(tests, coverageInfo, changes).ToList();

        ICoverageRecalculator coverageRecalculator =
            new CoverageRecalculator(coverageRepository, testRunner, coverageExtractor);
        
        coverageRecalculator.Recalculate(testsThatDependsOnChanges);

        return testsThatDependsOnChanges.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}