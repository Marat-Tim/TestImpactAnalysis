using System.Diagnostics;
using System.Text;

namespace TestImpactAnalysis.Coverage.Impl;

public class CmdTestRunner : ITestRunner
{
    private const string TestResultsPath = "TestResults";

    private const string Command =
        "/C dotnet test --collect:\"XPlat Code Coverage;Format=json\" --filter \"FullyQualifiedName~{0}\"";

    private readonly string _projPath;
    
    public CmdTestRunner(string projPath)
    {
        _projPath = projPath;
    }

    public bool WriteOutput { private get; init; }

    public string RunAndGetRawCoverage(string test)
    {
        var testResultsFullPath = Path.Combine(_projPath, TestResultsPath);

        if (Directory.Exists(testResultsFullPath))
        {
            Directory.Delete(testResultsFullPath, true);
        }

        var startInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "cmd.exe",
            Arguments = string.Format(Command, test),
            WorkingDirectory = _projPath,
            RedirectStandardOutput = true,
            StandardOutputEncoding = Encoding.UTF8
        };

        if (WriteOutput)
        {
            Console.WriteLine($"Run test {test}");
        }
        
        string path;
        using (var process = new Process { StartInfo = startInfo })
        {
            process.Start();
            path = process.StandardOutput.ReadToEnd().Split("\n")[^2].Trim();
            process.WaitForExit();
        }
        var coverage = File.ReadAllText(path, Encoding.UTF8);
        return coverage;
    }
}