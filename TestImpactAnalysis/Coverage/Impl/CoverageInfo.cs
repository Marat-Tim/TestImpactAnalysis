using Microsoft.Extensions.Logging;

namespace TestImpactAnalysis.Coverage.Impl;

public class CoverageInfo : ICoverageInfo
{
    private readonly ICoverageExtractor _coverageExtractor;
    
    private readonly ILogger _logger;

    private readonly ICoverageRepository _coverageRepository;

    private readonly ITestRunner _testRunner;

    public CoverageInfo(ICoverageRepository coverageRepository,
        ITestRunner testRunner,
        ICoverageExtractor coverageExtractor,
        ILogger logger)
    {
        _coverageRepository = coverageRepository;
        _testRunner = testRunner;
        _coverageExtractor = coverageExtractor;
        _logger = logger;
    }

    public ISet<string> GetDependentMethods(string test)
    {
        if (_coverageRepository.Exists(test))
        {
            _logger.LogDebug($"Received coverage for {test} from repository");
            return _coverageRepository.GetCoverage(test);
        }

        var rawCoverage = _testRunner.RunAndGetRawCoverage(test);
        var coverage = _coverageExtractor.ExtractFromRowData(rawCoverage);
        _logger.LogDebug($"Test {test} finished and coverage has extracted");

        _coverageRepository.Save(test, coverage);
        _logger.LogDebug($"Saved coverage for test {test} in repository");

        return coverage;
    }
}