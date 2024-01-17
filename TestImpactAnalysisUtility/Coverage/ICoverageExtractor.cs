namespace TestImpactAnalysisUtility.Coverage;

public interface ICoverageExtractor
{
    ISet<string> ExtractFromRowData(string coverage);
}