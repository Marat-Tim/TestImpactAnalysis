using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestImpactAnalysisUtility.Tests.Impl;

public class MsTestTemplate : ITestTemplate
{
    public bool IsTest(MethodInfo methodInfo)
    {
        return methodInfo.DeclaringType.GetCustomAttribute<TestClassAttribute>() != null &&
               methodInfo.GetCustomAttribute<TestMethodAttribute>() != null;
    }
}