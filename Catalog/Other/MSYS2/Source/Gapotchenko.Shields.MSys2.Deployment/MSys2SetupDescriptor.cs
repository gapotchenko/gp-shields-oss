// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.MSys2.Deployment;

readonly record struct MSys2SetupDescriptor(string InstallationPath)
{
    public MSys2SetupInstanceAttributes Attributes { get; init; }

    public Version? Version { get; init; }
}
