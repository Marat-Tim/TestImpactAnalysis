using TestImpactAnalysis.Coverage.Impl;

namespace TestImpactAnalysis.Coverage;

public interface ICoverageExtractor
{
    ISet<FileCoverage> ExtractFromRowData(string coverage);
}