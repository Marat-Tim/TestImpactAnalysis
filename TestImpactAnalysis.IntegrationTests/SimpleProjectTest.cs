using System;
using System.IO;
using LibGit2Sharp;
using Xunit;
using Xunit.Abstractions;
using static TestImpactAnalysis.IntegrationTests.TestUtils;

namespace TestImpactAnalysis.IntegrationTests;

public class SimpleProjectTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SimpleProjectTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact(DisplayName = "Check, that program work correctly on SimpleProject")]
    public void FirstRunTest()
    {
        string resourcesPath = Path.Combine("Resources", "SimpleProject");
        if (Directory.Exists(resourcesPath))
        {
            throw new Exception($"{resourcesPath} directory has not been deleted, delete it manually");
        }
        try
        {
            CloneRepository("https://github.com/testimpactanalysis/SimpleProject.git", resourcesPath);
            string pathToDirectoryWithGit = resourcesPath;
            string pathToTestsProject = Path.Combine(pathToDirectoryWithGit, "SimpleProject.Tests");
            string pathToTestsDll =
                BuildProjectAndGetDll(Path.Combine(pathToTestsProject, "SimpleProject.Tests.csproj"));
            Assert.Equal(new[] { "SimpleProject.Tests.ATest.FooTest" },
                new Default.TestsThatDependsOnChanges(pathToTestsDll, pathToTestsProject, pathToDirectoryWithGit));
        }
        finally
        {
            try
            {
                DeleteRepository(resourcesPath);
            }
            catch (Exception ex)
            {
                _testOutputHelper.WriteLine($"Couldn't delete directory {resourcesPath}, delete it manually");
                _testOutputHelper.WriteLine(ex.Message);
            }
        }
    }
}