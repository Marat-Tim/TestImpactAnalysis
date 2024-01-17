namespace TestImpactAnalysisUtility.Coverage;

public interface ICoverageExtractor
{
    ISet<string> Extract(string coverage);
}