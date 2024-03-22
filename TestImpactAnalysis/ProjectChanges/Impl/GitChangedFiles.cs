using System.Collections;
using LibGit2Sharp;

namespace TestImpactAnalysis.ProjectChanges.Impl;

public class GitChangedFiles : IEnumerable<string>
{
    private readonly string _pathToDirectoryWithGit;
    
    private readonly string _commit1Hash;
    
    private readonly string _commit2Hash;

    public GitChangedFiles(string pathToDirectoryWithGit, string commit1Hash, string commit2Hash)
    {
        _pathToDirectoryWithGit = pathToDirectoryWithGit;
        _commit1Hash = commit1Hash;
        _commit2Hash = commit2Hash;
    }
    
    public IEnumerator<string> GetEnumerator()
    {
        using var repo = new Repository(_pathToDirectoryWithGit);
        var commit1 = repo.Lookup<Commit>(_commit1Hash);
        var commit2 = repo.Lookup<Commit>(_commit2Hash);
        foreach (TreeEntryChanges change in repo.Diff.Compare<TreeChanges>(commit1.Tree, commit2.Tree))
        {
            yield return change.Path.StandardizePath();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}