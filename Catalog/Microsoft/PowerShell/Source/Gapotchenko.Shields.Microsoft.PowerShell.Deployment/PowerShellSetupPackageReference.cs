// Gapotchenko.Shields.Microsoft.PowerShell.Deployment
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Microsoft.PowerShell.Deployment;

sealed class PowerShellSetupPackageReference(string id) : IPowerShellSetupPackageReference
{
    public string Id => id;
}
