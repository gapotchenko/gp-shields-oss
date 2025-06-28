// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.IO;
using Gapotchenko.FX.Linq;

namespace Gapotchenko.Shields.Homebrew.Deployment;

sealed class BrewSetupInstanceImpl(
    string installationPath,
    string productPath,
    Lazy<Version> version,
    BrewSetupInstanceAttributes attributes,
    string? cellarPath,
    string? repositoryPath) :
    IBrewSetupInstance,
    IFormattable
{
    public string DisplayName => $"Homebrew {BrewVersion.GetDisplayName(Version)}";

    public Version Version => version.Value;

    public string InstallationPath { get; } = Path.TrimEndingDirectorySeparator(installationPath);

    public string ProductPath => productPath;

    public string ResolvePath(string? relativePath)
    {
        if (!string.IsNullOrEmpty(relativePath))
            relativePath = TranslatePath(relativePath);

        string path = Path.GetFullPath(Path.Combine(InstallationPath, relativePath ?? string.Empty));
        if (relativePath == null)
            path += Path.DirectorySeparatorChar;
        return path;
    }

    string TranslatePath(string path)
    {
        var parts = FileSystem.SplitPath(path).Memoize();
        if (cellarPath != null && parts.StartsWith(["Cellar"], FileSystem.PathComparer))
            return Path.Combine([cellarPath, .. parts.Skip(1)]);
        if (parts.StartsWith(["Homebrew"], FileSystem.PathComparer))
            return Path.Combine([RepositoryPath, .. parts.Skip(1)]);
        return path;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    string RepositoryPath => repositoryPath ?? DefaultRepositoryPath;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    string DefaultRepositoryPath =>
        RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.OSArchitecture == Architecture.Arm64
            ? InstallationPath
            : Path.Combine(InstallationPath, "Homebrew");

    public BrewSetupInstanceAttributes Attributes => attributes;

    #region Formatting

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString(format);

    public string ToString(string? format) =>
        format switch
        {
            "G" or null => ToString()!,
            "D" => DisplayName,
            _ => throw new FormatException("Format specifier was invalid.")
        };

    #endregion
}
