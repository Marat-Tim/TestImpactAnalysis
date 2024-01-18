using System.Text.Json;

namespace TestImpactAnalysisUtility.Coverage.Impl;

public class JsonCoverageExtractor : ICoverageExtractor
{
    public ISet<string> ExtractFromRowData(string coverage)
    {
        var json = JsonSerializer.Deserialize<
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, MethodCoverage>>>>
        >(coverage);

        ISet<string> files = new HashSet<string>();

        foreach (var (dll, fileToClass) in json)
        {
            foreach (var (file, classToMethod) in fileToClass)
            {
                foreach (var (clazz, methodToCoverage) in classToMethod)
                {
                    foreach (var (method, coverageInfo) in methodToCoverage)
                    {
                        if (coverageInfo.IsCovered)
                        {
                            files.Add(file.Replace(@"\\", @"\"));
                        }
                    }
                }
            }
        }

        return files;
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