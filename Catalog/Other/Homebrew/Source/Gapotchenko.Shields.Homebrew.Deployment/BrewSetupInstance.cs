// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Math.Intervals;

namespace Gapotchenko.Shields.Homebrew.Deployment;

static class BrewSetupInstance
{
    internal static IBrewSetupInstance? TryCreate(string installationPath, IEnumerable<BrewSetupDescriptor> descriptors, Interval<Version> versions)
    {
        throw new NotImplementedException();
    }
}
