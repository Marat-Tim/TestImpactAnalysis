namespace TestImpactAnalysis.Coverage;

public interface ITestRunner
{
    string RunAndGetRawCoverage(string test);
}