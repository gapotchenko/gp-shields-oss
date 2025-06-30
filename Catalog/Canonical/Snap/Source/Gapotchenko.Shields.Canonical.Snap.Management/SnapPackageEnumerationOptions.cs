// Gapotchenko.Shields.Canonical.Snap
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Canonical.Snap.Management;

/// <summary>
/// Defines options for Snap package enumeration operations.
/// </summary>
[Flags]
public enum SnapPackageEnumerationOptions
{
    /// <summary>
    /// No options are specified.
    /// This is the default value.
    /// </summary>
    None = 0,

    /// <summary>
    /// Enumerate only current package revisions.
    /// </summary>
    Current = 1 << 0
}
