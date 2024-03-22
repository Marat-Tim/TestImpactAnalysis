using System.Reflection;
using Xunit;

Assembly.LoadFrom(args[0]);
var controller = new XunitFrontController(AppDomainSupport.Denied, args[0]);
using var visitor = new TestDiscoverySink();
controller.Find(false, visitor, TestFrameworkOptions.ForDiscovery());
visitor.Finished.WaitOne();
var tests = visitor.TestCases.Select(
    testCase => testCase.TestMethod.Method.Type + "." + testCase.TestMethod.Method.Name).ToList();
visitor.Finished.Dispose();
foreach (var test in tests)
{
    Console.WriteLine(test);
}