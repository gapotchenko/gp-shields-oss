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
        // FUTURE
        // On other platforms, the product file name may be different.
        // For example, "brew.exe" on Windows if Homebrew ever supports Windows.
        return "brew";
    }
}
