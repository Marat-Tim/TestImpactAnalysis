namespace TestImpactAnalysisUtility.Coverage;

public interface ICoverageRepository
{
    void Save(string test, ISet<string> coverage);

    ISet<string> GetCoverage(string test);

    bool Exists(string test);
}