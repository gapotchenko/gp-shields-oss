// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.Shields.Homebrew.Deployment;

namespace Gapotchenko.Shields.Homebrew.Management;

sealed class BrewManager(IBrewSetupInstance setupInstance) : IBrewManager
{
    public IBrewPackageManagement Formulae => field ??= CreatePackageManagement("Cellar");

    public IBrewPackageManagement Casks => field ??= CreatePackageManagement("Caskroom");

    IBrewPackageManagement CreatePackageManagement(string path) =>
        new BrewPackageManagement(this, setupInstance.ResolvePath(path));

    public IBrewSetupInstance Setup => setupInstance;
}
