// Gapotchenko.Shields.Microsoft.Wsl
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Threading;

namespace Gapotchenko.Shields.Microsoft.Wsl.Runtime;

/// <summary>
/// Provides runtime services for WSL.
/// </summary>
public static class WslRuntime
{
    /// <summary>
    /// Gets the instance of the currently running WSL,
    /// or <see langword="null"/> if the current app is not being run under WSL.
    /// </summary>
    public static IWslRunningInstance? RunningInstance => m_CachedRunningInstance.Value;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static EvaluateOnce<IWslRunningInstance?> m_CachedRunningInstance = new(GetRunningInstanceCore);

    static IWslRunningInstance? GetRunningInstanceCore()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return null;

        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WSL_INTEROP")))
        {
            string? distributionName = Environment.GetEnvironmentVariable("WSL_DISTRO_NAME");
            if (!string.IsNullOrEmpty(distributionName))
                return new WslRunningInstance(distributionName);
        }

        return null;
    }
}
