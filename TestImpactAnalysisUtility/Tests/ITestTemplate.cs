using System.Reflection;

namespace TestImpactAnalysisUtility.Tests;

public interface ITestTemplate
{
    bool IsTest(MethodInfo methodInfo);
}