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
    
    private readonly string _pathToWorkProject;
    
    private readonly string _repositoryFileName;

    public TestsThatDependsOnChanges(string pathToTestsDll, string pathToTestsProject, string pathToWorkProject, 
        string repositoryFileName = "coverage.json")
    {
        _pathToTestsDll = pathToTestsDll;
        _pathToTestsProject = pathToTestsProject;
        _pathToWorkProject = pathToWorkProject;
        _repositoryFileName = repositoryFileName;
    }
    
    public IEnumerator<string> GetEnumerator()
    {
        IEnumerable<string> tests = new TestListByDllProcessing(_pathToTestsDll, new MsTestTemplate());

        ICoverageRepository coverageRepository = new InFileCoverageRepository(
            Path.Combine(_pathToTestsProject, _repositoryFileName));

        ITestRunner testRunner = new CmdTestRunner(_pathToWorkProject);

        ICoverageExtractor coverageExtractor = new JsonCoverageExtractor(_pathToWorkProject);

        ICoverageInfo coverageInfo = new CoverageInfo(coverageRepository, testRunner, coverageExtractor);

        IChanges changes =
            new ChangesUsingChangedLines(new FileToChangedLinesBetweenTwoLastGitCommits(_pathToWorkProject));

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