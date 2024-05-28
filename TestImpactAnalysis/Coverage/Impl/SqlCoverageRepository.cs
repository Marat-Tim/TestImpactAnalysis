using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TestImpactAnalysis.Database;
using AppContext = TestImpactAnalysis.Database.AppContext;

namespace TestImpactAnalysis.Coverage.Impl;

public class SqlCoverageRepository : ICoverageRepository
{
    private readonly AppContext _context; 

    public SqlCoverageRepository(string connection, DatabaseType databaseType)
    {
        _context = new AppContext(connection, databaseType);
    }
    
    public void Save(string test, ISet<string> coverage)
    {
        var existingCoverage = _context.TestCoverages.FirstOrDefault(tc => tc.Test == test);

        if (existingCoverage != null)
        {
            existingCoverage.Coverage = new HashSet<CoverageUnit>();
            foreach (var item in coverage)
            {
                existingCoverage.Coverage.Add(new CoverageUnit { Uri = item });
            }
        }
        else
        {
            var newCoverage = new TestCoverage
            {
                Test = test,
                Coverage = coverage.Select(item => new CoverageUnit { Uri = item }).ToHashSet()
            };
            _context.TestCoverages.Add(newCoverage);
        }

        _context.SaveChanges();
    }

    public ISet<string> GetCoverage(string test)
    {
        return _context.CoverageUnits
            .Where(coverageUnit => coverageUnit.TestCoverage.Test == test)
            .Select(coverageUnit => coverageUnit.Uri)
            .ToHashSet();
    }

    public bool Exists(string test)
    {
        return _context.TestCoverages.Any(tc => tc.Test == test);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}