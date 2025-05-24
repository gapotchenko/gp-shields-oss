// Gapotchenko.Shields.Microsoft.PowerShell.Deployment
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Microsoft.PowerShell.Deployment;

/// <summary>
/// A reference to a Microsoft PowerShell package.
/// </summary>
public interface IPowerShellSetupPackageReference
{
    /// <summary>
    /// Gets the general package identifier.
    /// </summary>
    string Id { get; }
}
