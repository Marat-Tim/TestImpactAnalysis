using TestImpactAnalysisUtility.Coverage;
using TestImpactAnalysisUtility.Coverage.Impl;
using TestImpactAnalysisUtility.Tests.Impl;

var pathToTestsDll = @"C:\Users\User\Desktop\Курсач\Код\AspStudy\AspStudyTests\bin\Debug\net6.0\AspStudyTests.dll";
var projectPath = @"C:\Users\User\Desktop\Курсач\Код\AspStudy\AspStudyTests";

var tests = new TestListByDllProcessing(pathToTestsDll, new MsTestTemplate());

ICoverageRepository coverageRepository = new InFileCoverageRepository("coverage.json");

ICoverageInfo coverageInfo =
    new CoverageInfo(coverageRepository, new CmdTestRunner(projectPath, false), new JsonCoverageExtractor());


foreach (var test in tests)
{
    Console.WriteLine(test);
    var coverage = coverageInfo.GetDependentFiles(test);
    foreach (var file in coverage)
    {
        Console.WriteLine($"\t{file}");
    }
}