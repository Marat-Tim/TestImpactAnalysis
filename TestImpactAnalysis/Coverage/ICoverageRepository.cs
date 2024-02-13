using TestImpactAnalysis.Coverage.Impl;

namespace TestImpactAnalysis.Coverage;

public interface ICoverageRepository
{
    void Save(string test, ISet<FileCoverage> coverage);

    ISet<FileCoverage> GetCoverage(string test);

    bool Exists(string test);
}