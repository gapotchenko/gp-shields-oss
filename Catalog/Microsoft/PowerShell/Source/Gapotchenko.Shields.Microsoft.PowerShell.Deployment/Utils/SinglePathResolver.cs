using Gapotchenko.FX.IO;

namespace Gapotchenko.Shields.Microsoft.PowerShell.Deployment.Utils;

readonly struct SinglePathResolver(string rootPath)
{
    public string RootPath { get; } = Path.TrimEndingDirectorySeparator(rootPath);

    public string ResolvePath(string? relativePath) =>
        PathUtil.ConstructPath(RootPath, relativePath);
}
