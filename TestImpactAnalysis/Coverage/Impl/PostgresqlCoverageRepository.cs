using Npgsql;

namespace TestImpactAnalysis.Coverage.Impl;

public class PostgresqlCoverageRepository : ICoverageRepository
{
     private readonly string _connectionString;

    public PostgresqlCoverageRepository(String connectionString)
    {
        _connectionString = connectionString;
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        var ddl = connection.CreateCommand();
        ddl.CommandText = @"
CREATE TABLE IF NOT EXISTS test_coverage (
    test text PRIMARY KEY 
);
CREATE TABLE IF NOT EXISTS coverage_unit (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    uri text,
    test_coverage_id int REFERENCES test_coverage(test)                                       
)";
        ddl.ExecuteNonQuery();
    }
    
    public void Save(string test, ISet<string> coverage)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        if (!Exists(test))
        {
            var insertTestCommand = connection.CreateCommand();
            insertTestCommand.CommandText = @"INSERT INTO test_coverage VALUES (@test)";
            insertTestCommand.Parameters.AddWithValue("@test", test);
            insertTestCommand.ExecuteNonQuery();
        }

        foreach (var uri in coverage)
        {
            InsertUri(uri, test, connection);
        }
    }

    private void InsertUri(string uri, string test, NpgsqlConnection connection)
    {
        var existsUriCommand = connection.CreateCommand();
        existsUriCommand.CommandText = @"SELECT 1 FROM coverage_unit WHERE uri = @uri";
        existsUriCommand.Parameters.AddWithValue("@uri", uri);
        using var reader = existsUriCommand.ExecuteReader();
        if (!reader.HasRows)
        {
            var insertUriCommand = connection.CreateCommand();
            insertUriCommand.CommandText = 
                @"INSERT INTO coverage_unit(uri, test_coverage_id) VALUES (@uri, @test_coverage_id)";
            insertUriCommand.Parameters.AddWithValue("@uri", uri);
            insertUriCommand.Parameters.AddWithValue("@test_coverage_id", test);
            insertUriCommand.ExecuteNonQuery();
        }
    }

    public ISet<string> GetCoverage(string test)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var getCommand = connection.CreateCommand();
        getCommand.CommandText = @"
SELECT uri FROM coverage_unit
WHERE test_coverage_id = @test";
        getCommand.Parameters.AddWithValue("@test", test);
        using var reader = getCommand.ExecuteReader();
        ISet<string> coverage = new HashSet<string>();
        while (reader.Read())
        {
            var uri = reader.GetString(0);
            coverage.Add(uri);
        }
        return coverage;
    }

    public bool Exists(string test)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var existsCommand = connection.CreateCommand();
        existsCommand.CommandText = @"SELECT 1 FROM test_coverage WHERE test = @name";
        existsCommand.Parameters.AddWithValue("@name", test);
        using var reader = existsCommand.ExecuteReader();
        return reader.HasRows;
    }
}