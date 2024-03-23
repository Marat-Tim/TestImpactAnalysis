using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TestImpactAnalysis.Database;

public class AppContext : DbContext
{
    private readonly string _connection;
    
    private readonly DatabaseType _databaseType;

    public DbSet<TestCoverage> TestCoverages { get; set; }
    
    public DbSet<CoverageUnit> CoverageUnits { get; set; }

    public AppContext(string connection, DatabaseType databaseType)
    {
        _connection = connection;
        _databaseType = databaseType;
        Database.EnsureCreated();
    }
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //TODO It doesn't look right
        switch (_databaseType)
        {
            case DatabaseType.PostgreSQL:
                optionsBuilder.UseNpgsql(_connection);
                break;
            case DatabaseType.SQLite:
                optionsBuilder.UseSqlite(_connection);
                break;
            default:
                throw new NotImplementedException("Unknown enum value");
        }
    }

    public override void Dispose()
    {
        if (_databaseType == DatabaseType.SQLite)
        {
            SqliteConnection.ClearAllPools();
        }
        base.Dispose();
    }
}