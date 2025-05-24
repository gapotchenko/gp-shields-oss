// Gapotchenko.Shields.Java.Deployment
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2019

using Gapotchenko.FX.IO;
using Gapotchenko.Shields.Java.Configuration;

namespace Gapotchenko.Shields.Java.Deployment;

class JavaSetupInstanceFS : IJavaSetupInstance
{
    public JavaSetupInstanceFS(string homePath, string? productIDHint)
    {
        HomePath = PathEx.TrimEndingDirectorySeparator(homePath);
        m_ProductIdHint = productIDHint;

        m_ReleaseManifest = new(GetReleaseManifestCore);
    }

    public string HomePath { get; }

    public Version Version => m_CachedVersion ??= GetVersionCore();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    Version? m_CachedVersion;

    Version GetVersionCore()
    {
        var releaseManifest =
            m_ReleaseManifest.Value ??
            throw new InvalidDataException("Java instance has no release manifest.");

        string s =
            releaseManifest["JAVA_VERSION"] ??
            throw new InvalidDataException("Java release manifest does not define version.");

        s = s.Trim('"').Replace('_', '.');

        return Version.Parse(s);
    }

    public IJavaSetupPackageReference Product =>
        m_CachedProduct ??=
        new JavaSetupPackageReferenceFS(HomePath, m_ProductIdHint, m_ReleaseManifest);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IJavaSetupPackageReference? m_CachedProduct;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    readonly string? m_ProductIdHint;

    public string ResolvePath(string? relativePath)
    {
        string path = Path.GetFullPath(Path.Combine(HomePath, relativePath ?? string.Empty));
        if (relativePath == null)
            path += Path.DirectorySeparatorChar;
        return path;
    }

    readonly Lazy<JavaProperties?> m_ReleaseManifest;

    JavaProperties? GetReleaseManifestCore()
    {
        string filePath = Path.Combine(HomePath, "release");
        if (!File.Exists(filePath))
            return null;

        return JavaProperties.Load(filePath);
    }

    public static JavaSetupInstanceFS? TryCreate(string? homePath, string? productIdHint)
    {
        if (!Directory.Exists(homePath))
            return null;
        return new JavaSetupInstanceFS(homePath, productIdHint);
    }
}
