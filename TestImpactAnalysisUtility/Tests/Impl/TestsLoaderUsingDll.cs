using System.Reflection;

namespace TestImpactAnalysisUtility.Tests.Impl;

public class TestsLoaderUsingDll : ITestsLoader
{
    private readonly string _testsDll;

    private readonly ITestTemplate _testTemplate;

    public TestsLoaderUsingDll(string testsDll, ITestTemplate testTemplate)
    {
        _testsDll = testsDll;
        _testTemplate = testTemplate;
    }

    public IList<string> Load()
    {
        IList<string> tests = new List<string>();

        foreach (var type in Assembly.LoadFrom(_testsDll).GetTypes())
        {
            if (type.FullName != null)
            {
                foreach (var methodInfo in type.GetMethods())
                {
                    if (_testTemplate.Check(methodInfo))
                    {
                        tests.Add($"{type.FullName}.{methodInfo.Name}");
                    }
                }
            }
        }

        return tests;
    }
}