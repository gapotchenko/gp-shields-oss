// Gapotchenko.Shields.Microsoft.PowerShell.Deployment
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.FX.IO;
using Gapotchenko.Shields.Microsoft.PowerShell.Deployment.Utils;

namespace Gapotchenko.Shields.Microsoft.PowerShell.Deployment;

sealed class PowerShellSetupInstance : IPowerShellSetupInstance
{
    public PowerShellSetupInstance(string installationPath, string productPath)
    {
        m_PathResolver = new(installationPath);
        ProductPath = PathEx.GetRelativePath(installationPath, productPath);
    }

    public string DisplayName => "Windows PowerShell";

    public string InstallationPath => m_PathResolver.RootPath;

    public string ProductPath { get; }

    public string ResolvePath(string? relativePath) => m_PathResolver.ResolvePath(relativePath);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    readonly SinglePathResolver m_PathResolver;

    public IPowerShellSetupPackageReference Product =>
        m_CachedProduct ??=
        new("Microsoft.PowerShell.Product");

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    PowerShellSetupPackageReference? m_CachedProduct;
}
