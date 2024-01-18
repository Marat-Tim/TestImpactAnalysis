using System.Collections;
using System.Diagnostics;

namespace TestImpactAnalysisUtility.ProjectChanges.Impl;

public class TwoLastCommitDiff : IEnumerable<string>
{
    private const string Command = "/C git diff --name-only HEAD HEAD~1";

    private readonly string _path;

    public TwoLastCommitDiff(string pathToDirWithGit)
    {
        _path = pathToDirWithGit;
    }

    public IEnumerator<string> GetEnumerator()
    {
        var startInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "cmd.exe",
            Arguments = Command,
            WorkingDirectory = _path,
            RedirectStandardOutput = true
        };

        using var process = new Process { StartInfo = startInfo };
        process.Start();
        var result = process.StandardOutput.ReadToEnd()
            .Split("\n")
            .Where(path => path != "")
            .Select(relativePath => Path.Combine(_path, relativePath))
            .GetEnumerator();
        process.WaitForExit();
        return result;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}