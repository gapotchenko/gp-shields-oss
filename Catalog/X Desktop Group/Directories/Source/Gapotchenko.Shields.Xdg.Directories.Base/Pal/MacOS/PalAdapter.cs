// Gapotchenko.Shields.Xdg.Directories.Base
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.Shields.Xdg.Directories.Base.Utils;

namespace Gapotchenko.Shields.Xdg.Directories.Base.Pal.MacOS;

#if NET
[SupportedOSPlatform("macos")]
#endif
sealed class PalAdapter : Unix.PalAdapter
{
    PalAdapter()
    {
    }

    public static PalAdapter Instance { get; } = new();

    public override string GetDataHome() => GetUserApplicationSupportLibraryPath();

    public override string GetDataDirectories() => GetSystemApplicationSupportLibraryPath();

    public override string GetConfigurationHome() => GetUserApplicationSupportLibraryPath();

    public override string GetConfigurationDirectories() =>
        DirectoryUtil.Combine(
            GetUserLibraryPath(PreferencesLibraryName),
            GetSystemApplicationSupportLibraryPath(),
            GetSystemLibraryPath(PreferencesLibraryName));

    const string PreferencesLibraryName = "Preferences";

    public override string GetStateHome() => GetUserApplicationSupportLibraryPath();

    public override string GetCacheHome() => GetUserLibraryPath("Caches");

    public override string GetRuntimeDirectory() => GetUserApplicationSupportLibraryPath();

    static string GetUserApplicationSupportLibraryPath() =>
        GetUserLibraryPath(ApplicationSupportLibraryName);

    static string GetUserLibraryPath(string name) =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Library",
            name);

    static string GetSystemApplicationSupportLibraryPath() =>
        GetSystemLibraryPath(ApplicationSupportLibraryName);

    const string ApplicationSupportLibraryName = "Application Support";

    static string GetSystemLibraryPath(string name) => Path.Combine("/Library", name);
}
