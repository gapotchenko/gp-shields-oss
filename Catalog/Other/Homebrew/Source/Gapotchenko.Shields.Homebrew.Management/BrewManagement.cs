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
    /// Creates a manager for the specified Homebrew setup instance.
    /// </summary>
    /// <param name="setupInstance">The Homebrew setup instance.</param>
    /// <returns>An <see cref="IBrewManager"/> instance.</returns>
    public static IBrewManager CreateManager(IBrewSetupInstance setupInstance)
    {
        ArgumentNullException.ThrowIfNull(setupInstance);

        //return new BrewManager(setupInstance);

        throw new NotImplementedException();
    }
}
