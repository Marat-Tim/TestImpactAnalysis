using System.Collections;
using System.Diagnostics;
using System.Text;

namespace TestImpactAnalysis.Tests.Impl;

public class XunitTestList : IEnumerable<string>
{
    private readonly string _pathToTestsDll;

    public XunitTestList(string pathToTestsDll)
    {
        _pathToTestsDll = pathToTestsDll;
    }

    public IEnumerator<string> GetEnumerator()
    {
#warning Change working with paths
        //TODO Change working with paths

        var startInfo = new ProcessStartInfo()
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "./../../../../TestDiscoverer/bin/Debug/net6.0/TestDiscoverer.exe",
            Arguments = $"\"{_pathToTestsDll}\"",
            RedirectStandardOutput = true,
            StandardOutputEncoding = Encoding.UTF8
        };
        using var process = new Process { StartInfo = startInfo };
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}