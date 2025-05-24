// Gapotchenko.Shields.Canonical.Snap.Deployment
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.Shields.Canonical.Snap.Deployment.Utils;

namespace Gapotchenko.Shields.Canonical.Snap.Deployment;

sealed class SnapSetupInstance(string installationPath, string productPath, SnapSetupInstanceAttributes attributes) :
    ISnapSetupInstance
{
    public string InstallationPath => m_PathResolver.RootPath;

    public string ProductPath { get; } = productPath;

    public string ResolvePath(string? relativePath) => m_PathResolver.ResolvePath(relativePath);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    readonly SinglePathResolver m_PathResolver = new(installationPath);

    public SnapSetupInstanceAttributes Attributes { get; } = attributes;
}
