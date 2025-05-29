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
    public void UserDirectories_Registry_Enumerate()
    {
        var actual = XdgUserDirectory.Enumerate().Select(x => x.Name).ToList();
        var expected = actual.Order(StringComparer.Ordinal).ToList();
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void UserDirectories_Registry_Known()
    {
        foreach (var i in XdgUserDirectory.Enumerate())
            Assert.IsTrue(i.IsKnown);
    }

    [TestMethod]
    public void UserDirectories_Registry_Unknown()
    {
        Assert.IsFalse(XdgUserDirectory.FromName("PATH").IsKnown);
    }
}
