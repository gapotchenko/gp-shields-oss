// Gapotchenko.Shields.Microsoft.Wsl
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Microsoft.Wsl.Deployment;

/// <summary>
/// Flags for Microsoft WSL deployment discovery operations.
/// </summary>
public enum WslDiscoveryOptions
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
