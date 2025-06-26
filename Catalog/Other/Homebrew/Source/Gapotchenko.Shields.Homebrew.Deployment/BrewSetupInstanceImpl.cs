// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.IO;

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
        string path = Path.GetFullPath(Path.Combine(InstallationPath, relativePath ?? string.Empty));
        if (relativePath == null)
            path += Path.DirectorySeparatorChar;
        return path;
    }

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