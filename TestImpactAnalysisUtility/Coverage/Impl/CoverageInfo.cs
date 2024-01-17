namespace TestImpactAnalysisUtility.Coverage.Impl;

public class CoverageInfo : ICoverageInfo
{
    private readonly ICoverageExtractor _coverageExtractor;
    private readonly ICoverageRepository _coverageRepository;

    private readonly ITestRunner _testRunner;

    public CoverageInfo(ICoverageRepository coverageRepository,
        ITestRunner testRunner,
        ICoverageExtractor coverageExtractor)
    {
        _coverageRepository = coverageRepository;
        _testRunner = testRunner;
        _coverageExtractor = coverageExtractor;
    }

    public ISet<string> GetDependentFiles(string test)
    {
        if (_coverageRepository.Exists(test))
        {
            return _coverageRepository.GetCoverage(test);
        }

        var rawCoverage = _testRunner.Run(test);
        var coverage = _coverageExtractor.Extract(rawCoverage);

        _coverageRepository.Save(test, coverage);

        return coverage;
    }
}