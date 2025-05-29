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
    public void BaseDirectories_Registry_Enumerate()
    {
        var actual = XdgBaseDirectory.Enumerate().Select(x => x.Name).ToList();
        var expected = actual.Order(StringComparer.Ordinal).ToList();
        CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void BaseDirectories_Registry_Known()
    {
        foreach (var i in XdgBaseDirectory.Enumerate())
            Assert.IsTrue(i.IsKnown);
    }

    [TestMethod]
    public void BaseDirectories_Registry_Unknown()
    {
        Assert.IsFalse(XdgBaseDirectory.FromName("PATH").IsKnown);
    }
}
