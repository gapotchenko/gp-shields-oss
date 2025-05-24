// Gapotchenko.Shields.Microsoft.PowerShell.Deployment
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Microsoft.PowerShell.Deployment;

/// <summary>
/// Flags for Microsoft PowerShell deployment discovery operations.
/// </summary>
[Flags]
public enum PowerShellDiscoveryOptions
{
    /// <summary>
    /// No options specified.
    /// The default value.
    /// </summary>
    None = 0,

    /// <summary>
    /// Do not sort product setup instances by version.
    /// </summary>
    NoSort = 1 << 0
}
