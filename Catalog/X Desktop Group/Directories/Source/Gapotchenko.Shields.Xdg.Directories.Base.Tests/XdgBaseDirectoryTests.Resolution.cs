// Gapotchenko.Shields.Xdg.Directories
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.Base.Tests;

partial class XdgBaseDirectoryTests
{
    [TestMethod]
    public void BaseDirectories_Resolution_Known()
    {
        foreach (var i in XdgBaseDirectory.Enumerate())
            Assert.IsFalse(string.IsNullOrEmpty(i.ValueOrDefault));
    }

    [TestMethod]
    public void BaseDirectories_Resolution_Unknown()
    {
        Assert.IsNull(XdgBaseDirectory.FromName("PATH").ValueOrDefault);
    }
}
