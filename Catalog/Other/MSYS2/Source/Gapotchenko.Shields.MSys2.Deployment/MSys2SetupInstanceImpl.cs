// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.IO;

namespace Gapotchenko.Shields.MSys2.Deployment;

sealed class MSys2SetupInstanceImpl(Version version, string installationPath, string productPath) :
    IMSys2SetupInstance, IFormattable
{
    public string DisplayName => $"MSYS2 {version.Major}-{version.Minor:D2}-{version.Build:D2}";

    public Version Version => version;

    public string InstallationPath { get; } = Path.TrimEndingDirectorySeparator(installationPath);

    public string ProductPath => productPath;

    public string ResolvePath(string? relativePath)
    {
        string path = Path.GetFullPath(Path.Combine(InstallationPath, relativePath ?? string.Empty));
        if (relativePath == null)
            path += Path.DirectorySeparatorChar;
        return path;
    }

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
