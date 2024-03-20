using System.Collections;
using System.Reflection;
using Xunit;

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
        Assembly.LoadFrom(_pathToTestsDll);
        
        var controller = new XunitFrontController(AppDomainSupport.Denied, _pathToTestsDll, shadowCopy: true);
        using var visitor = new TestDiscoverySink();
        controller.Find(false, visitor, TestFrameworkOptions.ForDiscovery());
        visitor.Finished.WaitOne();
        var tests = visitor.TestCases.Select(testCase => testCase.DisplayName).ToList(); 
        visitor.Finished.Dispose();
        return tests.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}