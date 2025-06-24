// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Homebrew.Deployment;

readonly record struct BrewSetupDescriptor(string InstallationPath)
{
    public BrewSetupInstanceAttributes Attributes { get; init; }

    public string? ProductFileName { get; init; }

    public string? CellarPath { get; init; }

    public string? RepositoryPath { get; init; }
}
