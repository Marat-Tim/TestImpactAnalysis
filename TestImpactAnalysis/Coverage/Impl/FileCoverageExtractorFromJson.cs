using System.Reflection.Emit;
using System.Text.Json;

namespace TestImpactAnalysis.Coverage.Impl;

public class FileCoverageExtractorFromJson : ICoverageExtractor
{
    private readonly string _workingProjectPath;

    public FileCoverageExtractorFromJson(string workingProjectPath)
    {
        _workingProjectPath = workingProjectPath;
    }

    public ISet<string> ExtractFromRowData(string coverage)
    {
        var json = JsonSerializer.Deserialize<
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, MethodCoverage>>>>
        >(coverage);

        ISet<string> coveredFiles = new HashSet<string>();

        foreach (var (dll, fileToClass) in json)
        {
            foreach (var (file, classToMethod) in fileToClass)
            {
                if (IsFileCovered(classToMethod))
                {
                    coveredFiles.Add(Path.GetRelativePath(_workingProjectPath, file));
                }
            }
        }

        return coveredFiles;
    }

    private bool IsFileCovered(Dictionary<string, Dictionary<string, MethodCoverage>> classToMethod)
    {
        foreach (var (clazz, methodToCoverage) in classToMethod)
        {
            foreach (var (method, coverageInfo) in methodToCoverage)
            {
                if (coverageInfo.IsCovered)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private class MethodCoverage
    {
        // ReSharper disable once MemberCanBePrivate.Local
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public Dictionary<string, int>? Lines { get; set; }

        // ReSharper disable once UnusedMember.Local
        public List<BranchInfo>? Branches { get; set; }

        public bool IsCovered => Lines?.Values.Any(hits => hits != 0) ?? false;
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private class BranchInfo
    {
        public int Line { get; set; }

        public int Offset { get; set; }

        public int EndOffset { get; set; }

        public int Path { get; set; }

        public int Ordinal { get; set; }

        public int Hits { get; set; }
    }
}