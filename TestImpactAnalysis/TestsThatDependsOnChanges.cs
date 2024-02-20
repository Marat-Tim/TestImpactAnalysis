using System.Collections;
using TestImpactAnalysis.Coverage;
using TestImpactAnalysis.ProjectChanges;

namespace TestImpactAnalysis;

public class TestsThatDependsOnChanges : IEnumerable<string>
{
    private readonly ICoverageInfo _coverageInfo;
    
    private readonly IChanges _changes;

    private readonly IEnumerable<string> _tests;

    public TestsThatDependsOnChanges(IEnumerable<string> tests, ICoverageInfo coverageInfo, IChanges changes)
    {
        _tests = tests;
        _coverageInfo = coverageInfo;
        _changes = changes;
    }

    public IEnumerator<string> GetEnumerator()
    {
        foreach (var test in _tests)
        {
            var dependentFiles = _coverageInfo.GetDependentMethods(test);
            if (_changes.HasIntersection(dependentFiles))
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