// Gapotchenko.Shields.Canonical.Snap.Deployment
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Canonical.Snap.Deployment;

/// <summary>
/// Specifies the attributes of a Canonical Snap setup instance.
/// </summary>
[Flags]
public enum SnapSetupInstanceAttributes
{
    /// <summary>
    /// No attributes.
    /// </summary>
    None = 0,

    /// <summary>
    /// Setup instance has been deducted from <c>PATH</c> environment variable.
    /// </summary>
    Path = 1 << 0
}
