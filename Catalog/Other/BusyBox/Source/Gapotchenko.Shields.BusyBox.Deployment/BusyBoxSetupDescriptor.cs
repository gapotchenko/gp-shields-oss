// Gapotchenko.Shields.BusyBox
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.BusyBox.Deployment;

readonly record struct BusyBoxSetupDescriptor(string ProductPath)
{
    public BusyBoxSetupInstanceAttributes Attributes { get; init; }

    public string? InstallationPath { get; init; }

    public Version? Version { get; init; }

    public string? ManufacturerVersion { get; init; }

    public Architecture? Architecture { get; init; }

    public string KeyPath =>
        InstallationPath is { } installationPath
            ? Path.Combine(installationPath, ProductPath)
            : ProductPath;
}
