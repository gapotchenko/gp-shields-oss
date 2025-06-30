// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.Shields.Homebrew.Deployment;

namespace Gapotchenko.Shields.Homebrew.Management;

/// <summary>
/// Provides Homebrew management operations.
/// </summary>
public interface IBrewManager
{
    /// <summary>
    /// Gets the formula management interface.
    /// </summary>
    IBrewPackageManagement Formulae { get; }

    /// <summary>
    /// Gets the cask management interface.
    /// </summary>
    IBrewPackageManagement Casks { get; }

    /// <summary>
    /// Gets the setup instance this management interface is associated with.
    /// </summary>
    IBrewSetupInstance Setup { get; }
}
