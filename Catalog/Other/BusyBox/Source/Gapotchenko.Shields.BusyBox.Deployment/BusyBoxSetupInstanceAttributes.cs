// Gapotchenko.Shields.BusyBox
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.BusyBox.Deployment;

/// <summary>
/// Specifies the attributes of a BusyBox setup instance.
/// </summary>
[Flags]
public enum BusyBoxSetupInstanceAttributes
{
    /// <summary>
    /// No attributes.
    /// </summary>
    None = 0,

    /// <summary>
    /// Setup instance has been deducted from a product-specific environment variable.
    /// </summary>
    /// <remarks>
    /// Reserved for future use.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    Environment = 1 << 0,

    /// <summary>
    /// Setup instance has been deducted from <c>PATH</c> environment variable.
    /// </summary>
    Path = 1 << 1,

    /// <summary>
    /// Setup instance supports Unicode.
    /// </summary>
    Unicode = 1 << 2
}
