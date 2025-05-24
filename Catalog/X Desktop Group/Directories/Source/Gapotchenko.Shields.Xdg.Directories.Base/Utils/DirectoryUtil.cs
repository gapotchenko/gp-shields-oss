namespace Gapotchenko.Shields.Xdg.Directories.Base.Utils;

static class DirectoryUtil
{
    public static string Combine(params IEnumerable<string> directories) =>
        string.Join(
#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
            Path.PathSeparator,
#else
            new string(Path.PathSeparator, 1),
#endif
            directories);
}
