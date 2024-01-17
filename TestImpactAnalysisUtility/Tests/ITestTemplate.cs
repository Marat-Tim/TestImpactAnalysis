using System.Reflection;

namespace TestImpactAnalysisUtility.Tests;

public interface ITestTemplate
{
    bool Check(MethodInfo methodInfo);
}