// Gapotchenko.Shields.Xdg.Directories.Base
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.Shields.Xdg.Directories.Base.Utils;

namespace Gapotchenko.Shields.Xdg.Directories.Base.Pal.Windows;

#if NET
[SupportedOSPlatform("windows")]
#endif
sealed class PalAdapter : IPalAdapter
{
    PalAdapter()
    {
    }

    public static PalAdapter Instance { get; } = new();

    public string GetDataHome() => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    public string GetDataDirectories() =>
        DirectoryUtil.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));

    public string GetConfigurationHome() => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    public string GetConfigurationDirectories() =>
        DirectoryUtil.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

    public string GetStateHome() => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    public string GetCacheHome() =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "cache");

    public string GetRuntimeDirectory() => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
}
