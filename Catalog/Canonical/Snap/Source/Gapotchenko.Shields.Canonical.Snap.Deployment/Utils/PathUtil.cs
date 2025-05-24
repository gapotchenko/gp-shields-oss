namespace Gapotchenko.Shields.Canonical.Snap.Deployment.Utils;

static class PathUtil
{
    public static string ConstructPath(string installationPath, string? relativePath)
    {
        string path = Path.GetFullPath(Path.Combine(installationPath, relativePath ?? string.Empty));

        if (relativePath == null)
            path += Path.DirectorySeparatorChar;

        return path;
    }
}
