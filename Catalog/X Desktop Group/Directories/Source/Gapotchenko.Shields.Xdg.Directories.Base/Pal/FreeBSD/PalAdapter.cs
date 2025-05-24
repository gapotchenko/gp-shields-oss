// Gapotchenko.Shields.Xdg.Directories.Base
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

#if NET

namespace Gapotchenko.Shields.Xdg.Directories.Base.Pal.FreeBSD;

[SupportedOSPlatform("freebsd")]
sealed class PalAdapter : Unix.PalAdapter
{
    PalAdapter()
    {
    }

    public static PalAdapter Instance { get; } = new();
}

#endif
