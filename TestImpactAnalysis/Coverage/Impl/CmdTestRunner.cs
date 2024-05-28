using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;

namespace TestImpactAnalysis.Coverage.Impl;

public class CmdTestRunner : ITestRunner
{
    private const string TestResultsPath = "TestResults";

    private const string Command =
        "test --collect:\"XPlat Code Coverage;Format=json\" --filter \"FullyQualifiedName~{0}\" -- " +
        "DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.IncludeTestAssembly=true";

    private readonly string _projPath;
    
    private readonly ILogger _logger;

    public CmdTestRunner(string projPath, ILogger logger)
    {
        _projPath = projPath;
        _logger = logger;
    }

    public string RunAndGetRawCoverage(string test)
    {
        var testResultsFullPath = Path.Combine(_projPath, TestResultsPath);

        if (Directory.Exists(testResultsFullPath))
        {
            Directory.Delete(testResultsFullPath, true);
            _logger.LogDebug("Delete test results directory");
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
            _logger.LogDebug($"Start process fileName={startInfo.FileName} args={startInfo.Arguments}");
            process.WaitForExit();
            _logger.LogDebug($"Finish process fileName={startInfo.FileName} args={startInfo.Arguments}");
            text = process.StandardOutput.ReadToEnd();
            path = (text.Split(Environment.NewLine).FirstOrDefault(str => str.Contains("coverage.json"))
                   ?? throw new Exception("File with coverage not found")).Trim();
        }

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug($"Run test {test}");
        }
        else if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace(text);
        }
        
        var coverage = File.ReadAllText(path, Encoding.UTF8);
        return coverage;
    }
}