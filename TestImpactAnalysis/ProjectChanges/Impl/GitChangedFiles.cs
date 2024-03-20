using System.Collections;
using LibGit2Sharp;

namespace TestImpactAnalysis.ProjectChanges.Impl;

public class GitChangedFiles : IEnumerable<string>
{
    private readonly string _pathToDirectoryWithGit;

    public GitChangedFiles(string pathToDirectoryWithGit)
    {
        _pathToDirectoryWithGit = pathToDirectoryWithGit;
    }
    
    public IEnumerator<string> GetEnumerator()
    {
        using var repo = new Repository(_pathToDirectoryWithGit);
        var old = repo.Head.Tip.Parents.First().Tree;
        var recent = repo.Head.Tip.Tree;
        foreach (TreeEntryChanges change in repo.Diff.Compare<TreeChanges>(old, recent))
        {
            yield return Path.GetRelativePath(Path.GetFullPath("."), Path.GetFullPath(change.Path));
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}