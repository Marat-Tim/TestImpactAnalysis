using TestImpactAnalysis.Coverage.Impl;

namespace TestImpactAnalysis.ProjectChanges.Impl;

public class ChangesUsingChangedLines : IChanges
{
    private readonly IDictionary<string, ISet<int>> _fileToChangedLines;

    public ChangesUsingChangedLines(IDictionary<string, ISet<int>> fileToChangedLines)
    {
        _fileToChangedLines = fileToChangedLines;
    }

    public bool HasIntersection(ISet<FileCoverage> coverage)
    {
        foreach (var (file, changedLines) in _fileToChangedLines)
        {
#warning надо поменять на словарь из файла в покрытие методов
            FileCoverage fileCoverage = coverage.FirstOrDefault(
                fileCoverage => Path.GetFullPath(fileCoverage.Path) == Path.GetFullPath(file),
                null);
            if (fileCoverage != null)
            {
                foreach (var line in changedLines)
                {
                    foreach (var methodCoverage in fileCoverage.MethodsCoverages)
                    {
                        if (line >= methodCoverage.Start && line <= methodCoverage.End)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}