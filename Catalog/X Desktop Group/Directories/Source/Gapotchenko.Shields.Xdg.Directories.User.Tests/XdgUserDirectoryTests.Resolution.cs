// Gapotchenko.Shields.Xdg.Directories
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.User.Tests;

partial class XdgUserDirectoryTests
{
    [TestMethod]
    public void UserDirectories_Resolution_Known()
    {
        foreach (var i in XdgUserDirectory.Enumerate())
            Assert.IsFalse(string.IsNullOrEmpty(i.ValueOrDefault));
    }

    [TestMethod]
    public void UserDirectories_Resolution_Unknown()
    {
        Assert.IsNull(XdgUserDirectory.FromName("PATH").ValueOrDefault);
    }
}
