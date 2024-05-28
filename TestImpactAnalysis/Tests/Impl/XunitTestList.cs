using System.Collections;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;

namespace TestImpactAnalysis.Tests.Impl;

public class XunitTestList : IEnumerable<string>
{
    private readonly string _pathToTestsDll;
    private readonly ILogger _logger;

    public XunitTestList(string pathToTestsDll, ILogger logger)
    {
        _pathToTestsDll = pathToTestsDll;
        _logger = logger;
    }

    public IEnumerator<string> GetEnumerator()
    {
#warning Change working with paths
        //TODO Change working with paths

        var startInfo = new ProcessStartInfo()
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "./../../../../TestDiscoverer/bin/Debug/net6.0/TestDiscoverer",
            Arguments = $"\"{_pathToTestsDll}\"",
            RedirectStandardOutput = true,
            StandardOutputEncoding = Encoding.UTF8
        };
        
        using var process = new Process { StartInfo = startInfo };
        _logger.LogDebug($"Start process fileName={startInfo.FileName} args={startInfo.Arguments}");
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        _logger.LogDebug($"Exit fileName={startInfo.FileName} args={startInfo.Arguments}");
        return output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}