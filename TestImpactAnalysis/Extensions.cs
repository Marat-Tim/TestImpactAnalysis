namespace TestImpactAnalysis;

public static class Extensions
{
    public static string StandardizePath(this string path)
    {
        return Path.GetRelativePath(Path.GetFullPath("."), Path.GetFullPath(path));
    }
}