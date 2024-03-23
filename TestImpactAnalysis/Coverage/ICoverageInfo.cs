namespace TestImpactAnalysis.Coverage;

public interface ICoverageInfo
{
    ISet<string> GetDependentMethods(string test);
}