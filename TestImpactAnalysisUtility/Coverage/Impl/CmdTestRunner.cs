using System.Diagnostics;
using System.Text;

namespace TestImpactAnalysisUtility.Coverage.Impl;

public class CmdTestRunner : ITestRunner
{
    private const string TestResultsPath = "TestResults";

    private const string Command =
        "/C dotnet test --collect:\"XPlat Code Coverage;Format=json\" --filter \"FullyQualifiedName~{0}\"";

    private readonly string _projPath;

    private readonly bool _writeOutput;

    public CmdTestRunner(string projPath, bool writeOutput = true)
    {
        _projPath = projPath;
        _writeOutput = writeOutput;
    }

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
            RedirectStandardOutput = !_writeOutput
        };

        using (var process = new Process { StartInfo = startInfo })
        {
            process.Start();
            process.WaitForExit();
        }

        var path = Directory.GetFiles(
            Directory.GetDirectories(testResultsFullPath)[0]
        )[0];
        var coverage = File.ReadAllText(path, Encoding.UTF8);
        return coverage;
    }
}