using System.Collections;
using System.Reflection;

namespace TestImpactAnalysisUtility.Tests.Impl;

public class TestListByDllProcessing : ITestList
{
    private readonly string _pathToTestsDll;

    private readonly ITestTemplate _testTemplate;

    public TestListByDllProcessing(string pathToTestsDll, ITestTemplate testTemplate)
    {
        _pathToTestsDll = pathToTestsDll;
        _testTemplate = testTemplate;
    }

    public IEnumerator<string> GetEnumerator()
    {
        var tests = new List<string>();

        foreach (var type in Assembly.LoadFrom(_pathToTestsDll).GetTypes())
        {
            if (type.FullName != null)
            {
                foreach (var methodInfo in type.GetMethods())
                {
                    if (_testTemplate.IsTest(methodInfo))
                    {
                        tests.Add($"{type.FullName}.{methodInfo.Name}");
                    }
                }
            }
        }

        return tests.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}