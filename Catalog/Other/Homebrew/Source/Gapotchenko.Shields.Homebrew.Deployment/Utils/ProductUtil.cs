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
        // The product file name may differ depending on a platform.
        // For example, it can be "brew.exe" on Windows (if Homebrew ever supports Windows).
        return "brew";
    }
}
