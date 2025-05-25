// Gapotchenko.Shields.Canonical.Snap
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Canonical.Snap.Management;

/// <summary>
/// Flags for snap package listing operations.
/// </summary>
[Flags]
public enum SnapPackageListingOptions
{
    /// <summary>
    /// No options are specified.
    /// This is the default value.
    /// </summary>
    None = 0,

    /// <summary>
    /// List only current package revisions.
    /// </summary>
    Current = 1 << 0
}
