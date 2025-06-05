// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.MSys2.Deployment;

/// <summary>
/// Flags for MSYS2 deployment discovery operations.
/// </summary>
[Flags]
public enum MSys2DiscoveryOptions
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
