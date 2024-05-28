using System.Collections;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;

namespace TestImpactAnalysis.ProjectChanges.Impl;

public class GitChangedFiles : IEnumerable<string>
{
    private readonly string _pathToDirectoryWithGit;
    
    private readonly string _commit1Hash;
    
    private readonly string _commit2Hash;
    
    private readonly ILogger _logger;

    public GitChangedFiles(string pathToDirectoryWithGit, string commit1Hash, string commit2Hash, ILogger logger)
    {
        _pathToDirectoryWithGit = pathToDirectoryWithGit;
        _commit1Hash = commit1Hash;
        _commit2Hash = commit2Hash;
        _logger = logger;
    }
    
    public IEnumerator<string> GetEnumerator()
    {
        using var repo = new Repository(_pathToDirectoryWithGit);
        var commit1 = repo.Lookup<Commit>(_commit1Hash);
        var commit2 = repo.Lookup<Commit>(_commit2Hash);
        foreach (TreeEntryChanges change in repo.Diff.Compare<TreeChanges>(commit1.Tree, commit2.Tree))
        {
            _logger.LogDebug($"Find changed file: {change.Path.StandardizePath()}");
            yield return change.Path.StandardizePath();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}