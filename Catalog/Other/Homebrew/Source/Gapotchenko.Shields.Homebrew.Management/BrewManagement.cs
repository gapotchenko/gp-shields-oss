// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.Shields.Homebrew.Deployment;

namespace Gapotchenko.Shields.Homebrew.Management;

/// <summary>
/// Provides management services for Homebrew package manager.
/// </summary>
public static class BrewManagement
{
    /// <summary>
    /// Creates a manager for the specified setup instance.
    /// </summary>
    /// <param name="setupInstance">The setup instance.</param>
    /// <returns>An <see cref="IBrewManager"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="setupInstance"/> is <see langword="null"/>.</exception>
    public static IBrewManager CreateManager(IBrewSetupInstance setupInstance)
    {
        ArgumentNullException.ThrowIfNull(setupInstance);
        return new BrewManager(setupInstance);
    }
}
