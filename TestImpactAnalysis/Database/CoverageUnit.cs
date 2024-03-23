namespace TestImpactAnalysis.Database;

public class CoverageUnit
{
    public long Id { get; set; }
    
    public string Uri { get; set; }
    
    public TestCoverage TestCoverage { get; set; }
}