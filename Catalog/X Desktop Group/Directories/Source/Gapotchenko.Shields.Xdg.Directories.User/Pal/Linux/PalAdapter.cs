// Gapotchenko.Shields.Xdg.Directories
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.User.Pal.Linux;

#if NET
[SupportedOSPlatform("linux")]
#endif
sealed class PalAdapter : Unix.PalAdapter
{
    PalAdapter()
    {
    }

    public static PalAdapter Instance { get; } = new();
}
