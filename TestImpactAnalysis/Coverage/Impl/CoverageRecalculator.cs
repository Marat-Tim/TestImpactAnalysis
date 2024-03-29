﻿namespace TestImpactAnalysis.Coverage.Impl;

public class CoverageRecalculator : ICoverageRecalculator
{
    private readonly ICoverageExtractor _coverageExtractor;

    private readonly ICoverageRepository _coverageRepository;

    private readonly ITestRunner _testRunner;

    public CoverageRecalculator(ICoverageRepository coverageRepository,
        ITestRunner testRunner,
        ICoverageExtractor coverageExtractor)
    {
        _coverageRepository = coverageRepository;
        _testRunner = testRunner;
        _coverageExtractor = coverageExtractor;
    }

    public void Recalculate(IEnumerable<string> tests)
    {
        foreach (var test in tests)
        {
            string rawCoverage = _testRunner.RunAndGetRawCoverage(test);
            var coverage = _coverageExtractor.ExtractFromRowData(rawCoverage);
            _coverageRepository.Save(test, coverage);
        }
    }
}