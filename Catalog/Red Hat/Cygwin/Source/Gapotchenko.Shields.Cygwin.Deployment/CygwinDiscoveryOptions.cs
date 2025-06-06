// Gapotchenko.Shields.Cygwin
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Cygwin.Deployment;

/// <summary>
/// Flags for Cygwin deployment discovery operations.
/// </summary>
[Flags]
public enum CygwinDiscoveryOptions
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
