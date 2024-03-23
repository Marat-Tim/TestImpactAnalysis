using TestImpactAnalysis.Database;
using static TestImpactAnalysis.IntegrationTests.TestUtils;

namespace TestImpactAnalysis.IntegrationTests;

public class SimpleProjectTest
{
    [Fact(DisplayName = "Check, that program work correctly on SimpleProject")]
    public void FirstRunTest()
    {
        string resourcesPath = Path.Combine("Resources", "SimpleProject");
        WithResourcesRemoval(resourcesPath, () =>
        {
            CloneRepository("https://github.com/testimpactanalysis/SimpleProject.git", resourcesPath);
            string commit1Hash = "151bb98a878fa7fb544c0e96f3735a118ba4fa2f";
            string commit2Hash = "62fb8c7767f085cc2836b781ac1a229133cc3702";
            string dbConnection = $"Data Source={Path.Combine(resourcesPath, "test.db")}";
            DatabaseType databaseType = DatabaseType.SQLite;
            string pathToDirectoryWithGit = resourcesPath;
            string pathToTestsProject = Path.Combine(pathToDirectoryWithGit, "SimpleProject.Tests");
            string pathToTestsDll =
                BuildProjectAndGetDll(Path.Combine(pathToTestsProject, "SimpleProject.Tests.csproj"));
            Assert.Equal(new[] { "SimpleProject.Tests.ATest.FooTest" },
                new Default.TestsThatDependsOnChanges(pathToTestsDll, pathToTestsProject, pathToDirectoryWithGit,
                    commit1Hash, commit2Hash,
                    dbConnection, databaseType));
        });
    }
}