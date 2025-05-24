// Gapotchenko.Shields.Canonical.Snap.Deployment
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Canonical.Snap.Deployment;

/// <summary>
/// Flags for Canonical Snap deployment discovery operations.
/// </summary>
[Flags]
public enum SnapDiscoveryOptions
{
    /// <summary>
    /// No options specified.
    /// This is the default value.
    /// </summary>
    None = 0,

    /// <summary>
    /// Do not sort or prioritize product setup instances by version and edition.
    /// </summary>
    NoSort = 1 << 0
}
