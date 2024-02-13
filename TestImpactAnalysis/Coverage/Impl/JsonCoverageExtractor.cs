using System.Text.Json;

namespace TestImpactAnalysis.Coverage.Impl;

public class JsonCoverageExtractor : ICoverageExtractor
{
    private readonly string _workingProjectPath;

    public JsonCoverageExtractor(string workingProjectPath)
    {
        _workingProjectPath = workingProjectPath;
    }
    
    public ISet<FileCoverage> ExtractFromRowData(string coverage)
    {
        var json = JsonSerializer.Deserialize<
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, MethodCoverage>>>>
        >(coverage);

        ISet<FileCoverage> files = new HashSet<FileCoverage>();

        foreach (var (dll, fileToClass) in json)
        {
            foreach (var (file, classToMethod) in fileToClass)
            {
                ISet<Impl.MethodCoverage> coveredMethods = new HashSet<Impl.MethodCoverage>();
                foreach (var (clazz, methodToCoverage) in classToMethod)
                {
                    foreach (var (method, coverageInfo) in methodToCoverage)
                    {
                        if (coverageInfo.IsCovered)
                        {
                            var coveredLines = coverageInfo.Lines.Keys.Select(int.Parse).ToArray();
                            coveredMethods.Add(new Impl.MethodCoverage(coveredLines.Min(),
                                coveredLines.Max()));
                        }
                    }
                }

                if (coveredMethods.Count() != 0)
                {
                    string path = Path.GetRelativePath(_workingProjectPath, 
                        file.Replace(@"\\", @"\"));
                    files.Add(new FileCoverage(path, coveredMethods));
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