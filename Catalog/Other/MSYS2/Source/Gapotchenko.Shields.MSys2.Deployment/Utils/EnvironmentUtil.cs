// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.MSys2.Deployment.Utils;

static class EnvironmentUtil
{
    public static Architecture? TryGetPreciseOSArchitecture()
    {
        Architecture? osArchitecture = RuntimeInformation.OSArchitecture;

#if !(NETSTANDARD2_1_OR_GREATER || NETCOREAPP)
        if (osArchitecture != Architecture.X86)
        {
            // Legacy .NET BCL can lie about the actual OS architecture.
            // For example, .NET Framework 4.8.1 says OS architecture is x64 while it is actually running on Windows arm64 in x86/x64 emulation mode.
            osArchitecture = null;
        }
#endif

        return osArchitecture;
    }
}
