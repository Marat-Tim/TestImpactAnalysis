namespace TestImpactAnalysis.ProjectChanges;

public interface IChanges
{
    bool HasIntersection(ISet<string> coverage);
}