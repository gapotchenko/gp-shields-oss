// Gapotchenko.Shields.Xdg.Directories
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.User.Pal;

static class PalServices
{
    public static IPalAdapter Adapter => AdapterOrDefault ?? throw new PlatformNotSupportedException();

    public static IPalAdapter? AdapterOrDefault => AdapterFactory.Instance;

    static class AdapterFactory
    {
        public static IPalAdapter? Instance { get; } = CreateInstance();

        static IPalAdapter? CreateInstance()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return Windows.PalAdapter.Instance;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return MacOS.PalAdapter.Instance;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return Linux.PalAdapter.Instance;
#if NET
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                return FreeBSD.PalAdapter.Instance;
#endif
            else
                return null;
        }
    }
}
