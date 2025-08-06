// Gapotchenko.Shields.BusyBox
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.BusyBox.Deployment.Utils;

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

    /// <summary>
    /// Gets a similarity index between the specified architectures.
    /// </summary>
    /// <param name="guest">The guest architecture.</param>
    /// <param name="host">The host architecture.</param>
    /// <returns>
    /// The similarity index.
    /// The <c>0</c> value signifies that the architectures are equal,
    /// the <see cref="int.MaxValue"/> value signifies that the architectures are totally dissimilar.
    /// </returns>
    public static int GetArchitectureSimilarity(Architecture guest, Architecture host)
    {
        if (guest == host)
            return 0;
        else if (AreSimilar(guest, host))
            return 1;
        else if (AreSimilar(host, guest))
            return 2;
        else
            return int.MaxValue;

        static bool AreSimilar(Architecture guest, Architecture host) =>
            (guest, host) is
                (Architecture.X86, Architecture.X64) or
                (Architecture.Arm, Architecture.Arm64);
    }
}
