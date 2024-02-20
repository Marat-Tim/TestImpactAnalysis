using LibGit2Sharp;

namespace TestImpactAnalysis.ProjectChanges.Impl;

public class FileToChangedLinesBetweenTwoLastGitCommits : Dictionary<string, ISet<int>>
{
    public FileToChangedLinesBetweenTwoLastGitCommits(string pathToDirWithGit)
    {
        using var repo = new Repository(pathToDirWithGit);
        var old = repo.Head.Tip.Parents.First().Tree;
        var recent = repo.Head.Tip.Tree;
        foreach (TreeEntryChanges change in repo.Diff.Compare<TreeChanges>(old, recent))
        {
            Patch patch = repo.Diff.Compare<Patch>(old, recent, new[] { change.Path });
            ISet<int> changedLines = new HashSet<int>();
            foreach (var patchEntryChanges in patch)
            {
                foreach (var addedLine in patchEntryChanges.AddedLines)
                {
                    changedLines.Add(addedLine.LineNumber);
                }

                foreach (var deletedLine in patchEntryChanges.DeletedLines)
                {
                    changedLines.Add(deletedLine.LineNumber);
                }
            }
            base.Add(change.Path, changedLines);
        }
    }
    
}