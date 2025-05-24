namespace Gapotchenko.Shields.Canonical.Snap.Deployment.Utils;

readonly struct SinglePathResolver
{
    public SinglePathResolver(string rootPath)
    {
        RootPath = PathEx.TrimEndingDirectorySeparator(rootPath);
    }

    public string RootPath { get; }

    public string ResolvePath(string? relativePath) =>
        PathUtil.ConstructPath(RootPath, relativePath);
}
