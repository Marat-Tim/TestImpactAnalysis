using System.Collections;
using TestImpactAnalysis.Coverage;
using TestImpactAnalysis.Coverage.Impl;
using TestImpactAnalysis.ProjectChanges;
using TestImpactAnalysis.ProjectChanges.Impl;
using TestImpactAnalysis.Tests.Impl;

namespace TestImpactAnalysis.Default;

public class TestsThatDependsOnChanges : IEnumerable<string>
{
    private readonly string _pathToTestsDll;
    
    private readonly string _pathToTestsProject;
    
    private readonly string _pathToDirectoryWithGit;
    
    private readonly string _repositoryFileName;
    
    private readonly CmdTestRunner.OutputDetalization _writeDebug;

    public TestsThatDependsOnChanges(string pathToTestsDll, string pathToTestsProject, string pathToDirectoryWithGit, 
        string repositoryFileName = "coverage.json",
        CmdTestRunner.OutputDetalization writeDebug = CmdTestRunner.OutputDetalization.None)
    {
        _pathToTestsDll = pathToTestsDll;
        _pathToTestsProject = pathToTestsProject;
        _pathToDirectoryWithGit = pathToDirectoryWithGit;
        _repositoryFileName = repositoryFileName;
        _writeDebug = writeDebug;
    }
    
    public IEnumerator<string> GetEnumerator()
    {
        IEnumerable<string> tests = new XunitTestList(_pathToTestsDll);

        ICoverageRepository coverageRepository = new InFileCoverageRepository(
            Path.Combine(_pathToTestsProject, _repositoryFileName));

        ITestRunner testRunner = new CmdTestRunner(_pathToDirectoryWithGit) { WriteOutput = _writeDebug };

        ICoverageExtractor coverageExtractor = new FileCoverageExtractorFromJson(_pathToDirectoryWithGit);

        ICoverageInfo coverageInfo = new CoverageInfo(coverageRepository, testRunner, coverageExtractor);

        IChanges changes = new Changes(new GitChangedFiles(_pathToDirectoryWithGit));

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