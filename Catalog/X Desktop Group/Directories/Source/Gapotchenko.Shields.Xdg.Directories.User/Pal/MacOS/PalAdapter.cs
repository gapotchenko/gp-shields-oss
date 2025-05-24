// Gapotchenko.Shields.Xdg.Directories.User
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.User.Pal.MacOS;

#if NET
[SupportedOSPlatform("macos")]
#endif
sealed class PalAdapter : Unix.PalAdapter
{
    PalAdapter()
    {
    }

    public static PalAdapter Instance { get; } = new();

    public override string GetVideosDirectory() => GetUserProfileDirectory("Movies");
}
