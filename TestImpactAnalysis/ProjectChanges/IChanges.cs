using TestImpactAnalysis.Coverage;
using TestImpactAnalysis.Coverage.Impl;

namespace TestImpactAnalysis.ProjectChanges;

public interface IChanges
{
    bool HasIntersection(ISet<string> coverage);
}