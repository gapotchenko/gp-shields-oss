// Gapotchenko.Shields.Canonical.Snap
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.Shields.Canonical.Snap.Deployment;

namespace Gapotchenko.Shields.Canonical.Snap.Management;

/// <summary>
/// Provides management services for Canonical Snap technology.
/// </summary>
public static class SnapManagement
{
    /// <summary>
    /// Creates a manager for the specified Canonical Snap setup instance.
    /// </summary>
    /// <param name="setupInstance">The Canonical Snap setup instance.</param>
    /// <returns>An <see cref="ISnapManager"/> instance.</returns>
    public static ISnapManager CreateManager(ISnapSetupInstance setupInstance)
    {
        ArgumentNullException.ThrowIfNull(setupInstance);

        return new SnapManager(setupInstance);
    }
}
