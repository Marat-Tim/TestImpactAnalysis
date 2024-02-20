using TestImpactAnalysis.Coverage.Impl;

namespace TestImpactAnalysis.Coverage;

public interface ICoverageInfo
{
    ISet<FileCoverage> GetDependentMethods(string test);
}