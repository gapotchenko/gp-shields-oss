// Gapotchenko.Shields.Microsoft.Wsl
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.IO;

namespace Gapotchenko.Shields.Microsoft.Wsl.Deployment;

sealed class WslSetupInstance(string installationPath, Version version, string productPath) :
    IWslSetupInstance, IFormattable
{
    public string DisplayName => $"WSL {version}";

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

    public static IWslSetupInstance? TryCreate(string installationPath, Version version)
    {
        string productPath = "wsl.exe";
        if (!File.Exists(Path.Combine(installationPath, productPath)))
            return null;

        return new WslSetupInstance(installationPath, version, productPath);
    }
}
