namespace TestImpactAnalysis.Coverage;

public interface ICoverageRepository : IDisposable
{
    void Save(string test, ISet<string> coverage);

    ISet<string> GetCoverage(string test);

    bool Exists(string test);
}