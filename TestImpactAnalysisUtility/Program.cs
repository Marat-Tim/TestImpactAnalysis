using TestImpactAnalysisUtility.Coverage;
using TestImpactAnalysisUtility.Coverage.Impl;
using TestImpactAnalysisUtility.Tests.Impl;

var testsDll = @"C:\Users\User\Desktop\Курсач\Код\AspStudy\AspStudyTests\bin\Debug\net6.0\AspStudyTests.dll";
var projPath = @"C:\Users\User\Desktop\Курсач\Код\AspStudy\AspStudyTests";

var tests = new TestsLoaderUsingDll(testsDll, new MsTestTemplate()).Load();

ICoverageRepository coverageRepository = new InFileCoverageRepository("coverage.json");

ICoverageInfo coverageInfo =
    new CoverageInfo(coverageRepository, new CmdTestRunner(projPath, false), new JsonCoverageExtractor());


foreach (var test in tests)
{
    Console.WriteLine(test);
    var coverage = coverageInfo.GetDependentFiles(test);
    foreach (var file in coverage)
    {
        Console.WriteLine($"\t{file}");
    }
}