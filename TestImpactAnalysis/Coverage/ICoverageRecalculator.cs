namespace TestImpactAnalysis.Coverage;

public interface ICoverageRecalculator
{
    void Recalculate(IEnumerable<string> tests);
}