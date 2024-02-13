using System.Reflection;

namespace TestImpactAnalysis.Tests;

public interface ITestTemplate
{
    bool IsTest(MethodInfo methodInfo);
}