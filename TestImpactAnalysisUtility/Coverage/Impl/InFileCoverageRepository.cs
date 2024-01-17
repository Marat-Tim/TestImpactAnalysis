﻿using System.Text.Json;

namespace TestImpactAnalysisUtility.Coverage.Impl;

public class InFileCoverageRepository : ICoverageRepository
{
    private readonly string _path;

    private readonly IDictionary<string, ISet<string>> _testToCoverage;

    public InFileCoverageRepository(string path)
    {
        if (File.Exists(path))
        {
            var text = File.ReadAllText(path);
            _testToCoverage = JsonSerializer.Deserialize<Dictionary<string, ISet<string>>>(text) ??
                              new Dictionary<string, ISet<string>>();
        }
        else
        {
            _testToCoverage = new Dictionary<string, ISet<string>>();
        }

        _path = path;
    }

    public void Save(string test, ISet<string> coverage)
    {
        _testToCoverage[test] = coverage;
        SaveToFile();
    }

    public ISet<string> GetCoverage(string test)
    {
        return _testToCoverage[test];
    }

    public bool Exists(string test)
    {
        return _testToCoverage.ContainsKey(test);
    }

    private void SaveToFile()
    {
        var json = JsonSerializer.Serialize(_testToCoverage);
        File.WriteAllText(_path, json);
    }
}