namespace TestImpactAnalysisUtility.Coverage;

public interface ICoverageInfo
{
    ISet<string> GetDependentFiles(string test);
}