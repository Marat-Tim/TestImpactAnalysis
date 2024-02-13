namespace TestImpactAnalysis.Coverage.Impl;

public record FileCoverage(string Path, ISet<MethodCoverage> MethodsCoverages);