// Gapotchenko.Shields.Microsoft.PowerShell.Deployment
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.FX.Math.Intervals;

namespace Gapotchenko.Shields.Microsoft.PowerShell.Deployment;

/// <summary>
/// Provides deployment discovery services for Microsoft PowerShell.
/// </summary>
public static partial class PowerShellDeployment
{
    /// <summary>
    /// Gets the default setup instance of PowerShell.
    /// </summary>
    public static IPowerShellSetupInstance DefaultSetupInstance =>
        m_CachedDefaultSetupInstance ??=
        GetDefaultSetupInstanceCore();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static IPowerShellSetupInstance? m_CachedDefaultSetupInstance;

    static IPowerShellSetupInstance GetDefaultSetupInstanceCore() =>
        EnumerateSetupInstances(ValueInterval.Infinite<Version>()).FirstOrDefault() ??
        throw new Exception("PowerShell setup instance is not found.");

    /// <summary>
    /// Enumerates setup instances of Microsoft PowerShell.
    /// The instances are sorted by product version.
    /// The newest versions come first.
    /// </summary>
    /// <param name="versions">The interval of Microsoft PowerShell versions to enumerate.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>Sequence of setup instances of Microsoft PowerShell.</returns>
    public static IEnumerable<IPowerShellSetupInstance> EnumerateSetupInstances(
        ValueInterval<Version> versions,
        PowerShellDiscoveryOptions options = default)
    {
        if (versions.IsEmpty)
            return [];

        var query = EnumerateSetupInstancesCore(versions, options);

        if ((options & PowerShellDiscoveryOptions.NoSort) == 0)
        {
            // TODO
        }

        return query;
    }

    static IEnumerable<IPowerShellSetupInstance> EnumerateSetupInstancesCore(Interval<Version> versions, PowerShellDiscoveryOptions options)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return Pal.Windows.EnumerateSetupInstances(versions, options);
        else
            return [];
    }
}
