namespace TestImpactAnalysisUtility.Coverage;

public interface ITestRunner
{
    string RunAndGetRawCoverage(string test);
}