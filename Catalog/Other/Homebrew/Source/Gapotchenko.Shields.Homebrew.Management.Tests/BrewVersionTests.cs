namespace Gapotchenko.Shields.Homebrew.Management.Tests;

[TestClass]
public class BrewVersionTests
{
    [TestMethod]
    [DataRow("0.1", "0.1.0", 0)]
    [DataRow("0.1", "0.2", -1)]
    [DataRow("1.2.3", "1.2.2", 1)]
    [DataRow("1.2.4", "1.2.4.1", -1)]
    [DataRow("1.2.3", "1.2.3alpha4", 1)]
    [DataRow("1.2.3", "1.2.3beta2", 1)]
    [DataRow("1.2.3", "1.2.3rc3", 1)]
    [DataRow("1.2.3", "1.2.3-p34", -1)]
    public void Management_Version_Compare(string? a, string? b, int comparison) =>
        Compare(a, b, comparison);

    [TestMethod]
    [DataRow("HEAD", "1.2.3", 1)]
    [DataRow("HEAD-abcdef", "1.2.3", 1)]
    [DataRow("HEAD-fedcba", "1.2.3", 1)]
    [DataRow("HEAD-abcdef", "HEAD-fedcba", 0)]
    [DataRow("HEAD", "HEAD-fedcba", 0)]
    public void Management_Version_CompareHead(string? a, string? b, int comparison) =>
        Compare(a, b, comparison);

    [TestMethod]
    [DataRow("1.2.3alpha", "1.2.3", -1)]
    [DataRow("1.2.3", "1.2.3a", -1)]
    [DataRow("1.2.3alpha4", "1.2.3a4", 0)]
    [DataRow("1.2.3alpha4", "1.2.3A4", 0)]
    [DataRow("1.2.3alpha4", "1.2.3alpha3", 1)]
    [DataRow("1.2.3alpha4", "1.2.3alpha5", -1)]
    [DataRow("1.2.3alpha4", "1.2.3alpha10", -1)]
    [DataRow("1.2.3alpha4", "1.2.3beta2", -1)]
    [DataRow("1.2.3alpha4", "1.2.3rc3", -1)]
    [DataRow("1.2.3alpha4", "1.2.3", -1)]
    [DataRow("1.2.3alpha4", "1.2.3-p34", -1)]
    public void Management_Version_CompareAlpha(string? a, string? b, int comparison) =>
        Compare(a, b, comparison);

    static void Compare(string? a, string? b, int comparison)
    {
        var versionA = BrewVersion.Parse(a);
        var versionB = BrewVersion.Parse(b);

        DoCompare(versionA, versionB, comparison);
        if (comparison != 0)
            DoCompare(versionB, versionA, -comparison);

        //var equalityComparer = EqualityComparer<BrewVersion>.Default;
        //if (comparison == 0)
        //{
        //    Assert.IsTrue(equalityComparer.Equals(versionA, versionB));
        //    if (versionA is not null && versionB is not null)
        //        Assert.AreEqual(equalityComparer.GetHashCode(versionA), equalityComparer.GetHashCode(versionB));
        //}

        static void DoCompare(BrewVersion? a, BrewVersion? b, int comparison)
        {
            Assert.AreEqual(
                comparison,
                Math.Sign(Comparer<BrewVersion>.Default.Compare(a, b)));
        }
    }
}
