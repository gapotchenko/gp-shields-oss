// Gapotchenko.Shields.Xdg.Directories.Base
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.Shields.Xdg.Directories.Base.Utils;

namespace Gapotchenko.Shields.Xdg.Directories.Base.Pal.Unix;

#if NET
[SupportedOSPlatform("macos")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("freebsd")]
#endif
abstract class PalAdapter : IPalAdapter
{
    public virtual string GetDataHome() =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".local",
            "share");

    public virtual string GetDataDirectories() =>
        DirectoryUtil.Combine(
            "/usr/local/share",
            "/usr/share");

    public virtual string GetConfigurationHome() =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".config");

    public virtual string GetConfigurationDirectories() => "/etc/xdg";

    public virtual string GetStateHome() =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".local",
            "state");

    public virtual string GetCacheHome() =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".cache");

    public virtual string GetRuntimeDirectory() => Invariant($"/run/user/{NativeMethods.getuid()}");
}
