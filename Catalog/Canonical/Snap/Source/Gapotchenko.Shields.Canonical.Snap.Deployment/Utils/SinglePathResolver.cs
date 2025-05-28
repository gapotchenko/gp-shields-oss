namespace Gapotchenko.Shields.Canonical.Snap.Deployment.Utils;

readonly struct SinglePathResolver(string rootPath)
{
    public string RootPath { get; } = Path.TrimEndingDirectorySeparator(rootPath);

    public string ResolvePath(string? relativePath) => PathUtil.ConstructPath(RootPath, relativePath);
}
