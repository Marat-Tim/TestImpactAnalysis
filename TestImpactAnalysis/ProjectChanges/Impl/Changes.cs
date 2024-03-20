using LibGit2Sharp;

namespace TestImpactAnalysis.ProjectChanges.Impl;

public class Changes : IChanges
{
    private readonly ISet<string> _files;

    public Changes(IEnumerable<string> changedFiles)
    {
        _files = changedFiles.ToHashSet();
    }
    
    public bool HasIntersection(ISet<string> coverage)
    {
        return _files.Overlaps(coverage);
    }
}