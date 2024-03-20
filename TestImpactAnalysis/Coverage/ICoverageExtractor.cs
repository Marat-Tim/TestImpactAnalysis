﻿using TestImpactAnalysis.Coverage.Impl;

namespace TestImpactAnalysis.Coverage;

public interface ICoverageExtractor
{
    ISet<string> ExtractFromRowData(string coverage);
}