using System.ComponentModel.DataAnnotations;

namespace TestImpactAnalysis.Database;

public class TestCoverage
{
    [Key]
    public string Test { get; set; }

    public ISet<CoverageUnit> Coverage { get; set; }
}