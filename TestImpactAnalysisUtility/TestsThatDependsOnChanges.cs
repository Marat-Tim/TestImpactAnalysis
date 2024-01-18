using System.Collections;
using TestImpactAnalysisUtility.Coverage;

namespace TestImpactAnalysisUtility;

public class TestsThatDependsOnChanges : IEnumerable<string>
{
    private readonly ISet<string> _changes;

    private readonly ICoverageInfo _coverageInfo;
    
    private readonly IEnumerable<string> _tests;

    public TestsThatDependsOnChanges(IEnumerable<string> tests, ICoverageInfo coverageInfo, ISet<string> changes)
    {
        _tests = tests;
        _coverageInfo = coverageInfo;
        _changes = changes.Select(Path.GetFullPath).ToHashSet();
    }

    public IEnumerator<string> GetEnumerator()
    {
        foreach (var test in _tests)
        {
            var dependentFiles = _coverageInfo.GetDependentFiles(test)
                .Select(Path.GetFullPath).ToHashSet();
            if (_changes.Overlaps(dependentFiles))
            {
                yield return test;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}