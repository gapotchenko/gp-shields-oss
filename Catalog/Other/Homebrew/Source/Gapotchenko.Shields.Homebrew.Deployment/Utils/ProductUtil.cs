// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Homebrew.Deployment.Utils;

static class ProductUtil
{
    public static string GetProductFileName()
    {
        // On other platforms, it may be different.
        // For example, "brew.exe" on Windows if Homebrew ever supports Windows.
        return "brew";
    }
}
