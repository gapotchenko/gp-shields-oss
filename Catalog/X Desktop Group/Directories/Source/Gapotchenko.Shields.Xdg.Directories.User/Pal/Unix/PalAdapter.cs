// Gapotchenko.Shields.Xdg.Directories.User
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.User.Pal.Unix;

#if NET
[SupportedOSPlatform("macos")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("freebsd")]
#endif
abstract class PalAdapter : IPalAdapter
{
    public virtual string GetDesktopDirectory() => GetUserProfileDirectory("Desktop");

    public virtual string GetDownloadsDirectory() => GetUserProfileDirectory("Downloads");

    public virtual string GetDocumentsDirectory() => GetUserProfileDirectory("Documents");

    public virtual string GetMusicDirectory() => GetUserProfileDirectory("Music");

    public virtual string GetPicturesDirectory() => GetUserProfileDirectory("Pictures");

    public virtual string GetVideosDirectory() => GetUserProfileDirectory("Videos");

    public virtual string GetTemplatesDirectory() => GetUserProfileDirectory("Templates");

    public virtual string GetPublicDirectory() => GetUserProfileDirectory("Public");

    protected static string GetUserProfileDirectory(string name) =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            name);
}
