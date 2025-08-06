// Gapotchenko.Shields.BusyBox
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.IO;

namespace Gapotchenko.Shields.BusyBox.Deployment;

sealed class BusyBoxSetupInstanceImpl(
    string installationPath,
    string productPath,
    Architecture architecture,
    Lazy<Version> version,
    string? manufacturerVersion,
    BusyBoxSetupInstanceAttributes attributes) :
    IBusyBoxSetupInstance,
    IFormattable
{
    public string DisplayName => $"BusyBox {BusyBoxVersion.GetDisplayName(Version)}";

    public string ManufacturerVersion => manufacturerVersion ?? BusyBoxVersion.GetDisplayName(Version);

    public Version Version => m_Version.Value;

    public Architecture Architecture => architecture;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    readonly Lazy<Version> m_Version = version;

    public string InstallationPath { get; } = Path.TrimEndingDirectorySeparator(installationPath);

    public string ProductPath { get; } = productPath;

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
        if (path == "busybox")
            return ProductPath;
        else
            return path;
    }

    public BusyBoxSetupInstanceAttributes Attributes { get; } = attributes;

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
