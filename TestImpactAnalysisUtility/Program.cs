using TestImpactAnalysisUtility;
using TestImpactAnalysisUtility.Coverage;
using TestImpactAnalysisUtility.Coverage.Impl;
using TestImpactAnalysisUtility.ProjectChanges.Impl;
using TestImpactAnalysisUtility.Tests.Impl;

var pathToTestsDll =   @"C:\Users\User\Desktop\Курсач\Код\Project\TestProject\bin\Debug\net6.0\TestProject.dll";
var projectPath =      @"C:\Users\User\Desktop\Курсач\Код\Project\TestProject";
var pathToDirWithGit = @"C:\Users\User\Desktop\Курсач\Код\Project";

var tests = new TestListByDllProcessing(pathToTestsDll, new MsTestTemplate()).ToArray();

ICoverageRepository coverageRepository = new InFileCoverageRepository("coverage.json");

ICoverageInfo coverageInfo =
    new CoverageInfo(coverageRepository, new CmdTestRunner(projectPath, false), new JsonCoverageExtractor());

ISet<string> changes = new TwoLastCommitDiff(pathToDirWithGit).ToHashSet();

foreach (string test in new TestsThatDependsOnChanges(tests, coverageInfo, changes))
{
    Console.WriteLine(test);
}