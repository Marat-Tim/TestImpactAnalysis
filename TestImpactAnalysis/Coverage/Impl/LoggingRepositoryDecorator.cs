using Microsoft.Extensions.Logging;

namespace TestImpactAnalysis.Coverage.Impl;

public class LoggingRepositoryDecorator : ICoverageRepository
{
    private readonly ICoverageRepository _coverageRepository;
    private readonly ILogger _logger;

    public LoggingRepositoryDecorator(ICoverageRepository coverageRepository, ILogger logger)
    {
        _coverageRepository = coverageRepository;
        _logger = logger;
    }
    
    public void Save(string test, ISet<string> coverage)
    {
        _coverageRepository.Save(test, coverage);
        _logger.LogDebug($"Save coverage for test {test}: {string.Join(',', coverage)}");
    }

    public ISet<string> GetCoverage(string test)
    {
        var coverage = _coverageRepository.GetCoverage(test);
        _logger.LogDebug($"Get coverage for test {test}: {string.Join(',', coverage)}");
        return coverage;
    }

    public bool Exists(string test)
    {
        bool exists = _coverageRepository.Exists(test);
        var word = exists ? "" : "not";
        _logger.LogDebug($"Checked, that coverage for test {test} {word} exists");
        return exists;
    }

    public void Dispose()
    {
        _coverageRepository.Dispose();
    }
}