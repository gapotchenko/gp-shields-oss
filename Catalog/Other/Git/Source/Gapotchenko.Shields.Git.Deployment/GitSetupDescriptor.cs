// Gapotchenko.Shields.Git
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Git.Deployment;

readonly record struct GitSetupDescriptor(string ProductPath)
{
    public GitSetupInstanceAttributes Attributes { get; init; }

    public string? InstallationPath { get; init; }

    public Version? Version { get; init; }

    public string? LibExecPath { get; init; }
}
