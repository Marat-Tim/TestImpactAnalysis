using System.Diagnostics;
using System.IO;
using LibGit2Sharp;

namespace TestImpactAnalysis.IntegrationTests;

public static class TestUtils
{
    public static string BuildProjectAndGetDll(string pathToCsproj)
    {
        var startInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "dotnet.exe",
            Arguments = $"build \"{pathToCsproj}\""
        };
        using Process process = new Process { StartInfo = startInfo };
        process.Start();
        process.WaitForExit();
        string projectName = Path.GetFileNameWithoutExtension(pathToCsproj);
        return Path.Combine(Path.GetDirectoryName(pathToCsproj), "bin", "Debug", "net6.0", $"{projectName}.dll");
    }
    
    public static void CloneRepository(string url, string path)
    {
        Repository.Clone(url, path);
    }

    public static void DeleteRepository(string path)
    {
        foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
        {
            File.SetAttributes(file, FileAttributes.Normal);
        }
        Directory.Delete(path, recursive: true);
    }
}