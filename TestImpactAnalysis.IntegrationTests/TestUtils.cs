using System.Diagnostics;
using LibGit2Sharp;

namespace TestImpactAnalysis.IntegrationTests;

public static class TestUtils
{
    public static string BuildProjectAndGetDll(string pathToCsproj)
    {
        var startInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "dotnet",
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

    public static void DeleteDirectory(string path)
    {
        foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
        {
            File.SetAttributes(file, FileAttributes.Normal);
        }
        Directory.Delete(path, recursive: true);
    }

    public static void WithResourcesRemoval(string resourcesPath, Action action)
    {
        if (Directory.Exists(resourcesPath))
        {
            try
            {
                DeleteDirectory(resourcesPath);
            }
            catch (IOException e)
            {
                throw new Exception($"{resourcesPath} directory has not been deleted, delete it manually", e);
            }
        }
        try
        {
            action();
        }
        finally
        {
            try
            {
                DeleteDirectory(resourcesPath);
            }
            catch (IOException e)
            {
                var pathToGit = Path.Combine(resourcesPath, ".git");
                if (Directory.Exists(pathToGit))
                {
                    throw new Exception($"Couldn't delete directory {pathToGit}, delete it manually", e);
                }
            }
        }
    }
}