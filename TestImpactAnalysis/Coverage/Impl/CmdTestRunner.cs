using System.Diagnostics;
using System.Text;

namespace TestImpactAnalysis.Coverage.Impl;

public class CmdTestRunner : ITestRunner
{
    private const string TestResultsPath = "TestResults";

    private const string Command =
        "test --collect:\"XPlat Code Coverage;Format=json\" --filter \"FullyQualifiedName~{0}\" -- " +
        "DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.IncludeTestAssembly=true";

    private readonly string _projPath;
    
    public CmdTestRunner(string projPath)
    {
        _projPath = projPath;
    }

    public OutputDetalization WriteOutput { private get; init; }

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
            FileName = "dotnet",
            Arguments = string.Format(Command, test),
            WorkingDirectory = _projPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8
        };

        string path, text;
        using (var process = new Process { StartInfo = startInfo })
        {
            process.Start();
            process.WaitForExit();
            text = process.StandardOutput.ReadToEnd();
            path = (text.Split(Environment.NewLine).FirstOrDefault(str => str.Contains("coverage.json"))
                   ?? throw new Exception("File with coverage not found")).Trim();
        }

        switch (WriteOutput)
        {
            case OutputDetalization.None:
                break;
            case OutputDetalization.Minimal:
                Console.WriteLine($"Run test {test}");
                break;
            case OutputDetalization.All:
                Console.WriteLine(text);
                break;
            default:
                throw new NotImplementedException("Unknown enum value");
        }
        var coverage = File.ReadAllText(path, Encoding.UTF8);
        return coverage;
    }

    public enum OutputDetalization
    {
        None, Minimal, All
    }
}