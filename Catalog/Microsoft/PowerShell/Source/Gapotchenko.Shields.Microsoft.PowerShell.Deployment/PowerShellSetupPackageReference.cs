// Gapotchenko.Shields.Microsoft.PowerShell
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Microsoft.PowerShell.Deployment;

sealed class PowerShellSetupPackageReference(string id) : IPowerShellSetupPackageReference
{
    public string Id => id;
}
