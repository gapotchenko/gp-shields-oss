// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
// Portions © Homebrew Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

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
    public void BrewVersion_Comparison(string? a, string? b, int comparison) => TestComparison(a, b, comparison);

    [TestMethod]
    [DataRow("HEAD", "1.2.3", 1)]
    [DataRow("HEAD-abcdef", "1.2.3", 1)]
    [DataRow("HEAD-fedcba", "1.2.3", 1)]
    [DataRow("HEAD-abcdef", "HEAD-fedcba", 0)]
    [DataRow("HEAD", "HEAD-fedcba", 0)]
    public void BrewVersion_Comparison_Head(string? a, string? b, int comparison) => TestComparison(a, b, comparison);

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
    public void BrewVersion_Comparison_Alpha(string? a, string? b, int comparison) =>
        TestComparison(a, b, comparison);

    [TestMethod]
    [DataRow("1.2.3beta2", "1.2.3b2", 0)]
    [DataRow("1.2.3beta2", "1.2.3B2", 0)]
    [DataRow("1.2.3beta2", "1.2.3beta1", 1)]
    [DataRow("1.2.3beta2", "1.2.3beta3", -1)]
    [DataRow("1.2.3beta2", "1.2.3beta10", -1)]
    [DataRow("1.2.3beta2", "1.2.3alpha4", 1)]
    [DataRow("1.2.3beta2", "1.2.3rc3", -1)]
    [DataRow("1.2.3beta2", "1.2.3", -1)]
    [DataRow("1.2.3beta2", "1.2.3-p34", -1)]
    public void BrewVersion_Comparison_Beta(string? a, string? b, int comparison) => TestComparison(a, b, comparison);

    [TestMethod]
    [DataRow("1.2.3pre9", "1.2.3PRE9", 0)]
    [DataRow("1.2.3pre9", "1.2.3pre8", 1)]
    [DataRow("1.2.3pre9", "1.2.3pre10", -1)]
    [DataRow("1.2.3pre3", "1.2.3alpha2", 1)]
    [DataRow("1.2.3pre3", "1.2.3alpha4", 1)]
    [DataRow("1.2.3pre3", "1.2.3beta3", 1)]
    [DataRow("1.2.3pre3", "1.2.3beta5", 1)]
    [DataRow("1.2.3pre3", "1.2.3rc2", -1)]
    [DataRow("1.2.3pre3", "1.2.3", -1)]
    [DataRow("1.2.3pre3", "1.2.3-p2", -1)]
    public void BrewVersion_Comparison_Prerelease(string? a, string? b, int comparison) => TestComparison(a, b, comparison);

    [TestMethod]
    [DataRow("1.2.3rc3", "1.2.3RC3", 0)]
    [DataRow("1.2.3rc3", "1.2.3rc2", 1)]
    [DataRow("1.2.3rc3", "1.2.3rc4", -1)]
    [DataRow("1.2.3rc3", "1.2.3rc10", -1)]
    [DataRow("1.2.3rc3", "1.2.3alpha4", 1)]
    [DataRow("1.2.3rc3", "1.2.3beta2", 1)]
    [DataRow("1.2.3rc3", "1.2.3", -1)]
    [DataRow("1.2.3rc3", "1.2.3-p34", -1)]
    public void BrewVersion_Comparison_ReleaseCandidate(string? a, string? b, int comparison) => TestComparison(a, b, comparison);

    [TestMethod]
    [DataRow("1.2.3.post34", "1.2.3.post33", 1)]
    [DataRow("1.2.3.post34", "1.2.3.post35", -1)]
    [DataRow("1.2.3.post34", "1.2.3.rc35", 1)]
    [DataRow("1.2.3.post34", "1.2.3.alpha35", 1)]
    [DataRow("1.2.3.post34", "1.2.3.beta35", 1)]
    [DataRow("1.2.3.post34", "1.2.3", 1)]
    public void BrewVersion_Comparison_Post(string? a, string? b, int comparison) => TestComparison(a, b, comparison);

    [TestMethod]
    [DataRow("2.1.0-p194", "2.1-p195", -1)]
    [DataRow("2.1-p194", "2.1.0-p195", -1)]
    [DataRow("2-p194", "2.1-p195", -1)]
    public void BrewVersion_Comparison_UnevenlyPadded(string? a, string? b, int comparison) => TestComparison(a, b, comparison);

    [TestMethod]
    [DataRow("2.1.0-p194", null, 1)]
    [DataRow(null, null, 0)]
    public void BrewVersion_Comparison_Null(string? a, string? b, int comparison) => TestComparison(a, b, comparison);

    [TestMethod]
    [DataRow(["R13B02-1", "R13B03", "R13B04", "R14B", "R14B01", "R14B02", "R14B03", "R14B04", "R15B01", "R15B02", "R15B03", "R15B03-1", "R16B"], DisplayName = $"{nameof(BrewVersion_Comparison_Order)}_Erlang")]
    public void BrewVersion_Comparison_Order(string?[] versions)
    {
        for (int i = 1; i < versions.Length; ++i)
            TestComparison(versions[i - 1], versions[i], -1);
    }

    [TestMethod]
    public void BrewVersion_Comparison_AnotherType()
    {
        const string s = "1.2.3";
        var version = new BrewVersion(s);
        Assert.ThrowsExactly<ArgumentException>(() => version.CompareTo(s));
    }

    // ----------------------------------------------------------------------

    static void TestComparison(string? a, string? b, int comparison)
    {
        var objA = BrewVersion.Parse(a);
        var objB = BrewVersion.Parse(b);

        CompareCore(objA, objB, comparison);
        if (comparison != 0)
        {
            // Check the inverse comparison.
            CompareCore(objB, objA, -comparison);

            // Check the equality.
            CompareCore(objA, objA, 0);
            if (a != b)
                CompareCore(objB, objB, 0);
        }

        static void CompareCore(BrewVersion? a, BrewVersion? b, int comparison)
        {
            #region Comparison

            Assert.AreEqual(
                comparison,
                Math.Sign(Comparer<BrewVersion>.Default.Compare(a, b)));

            if (a is not null)
            {
                Assert.AreEqual(comparison, Math.Sign(a.CompareTo(b)));
                Assert.AreEqual(comparison, Math.Sign(a.CompareTo((object?)b)));
            }
            if (b is not null)
            {
                Assert.AreEqual(comparison, -Math.Sign(b.CompareTo(a)));
                Assert.AreEqual(comparison, -Math.Sign(b.CompareTo((object?)a)));
            }

            Assert.AreEqual(comparison >= 0, a >= b);
            Assert.AreEqual(comparison <= 0, a <= b);
            Assert.AreEqual(comparison > 0, a > b);
            Assert.AreEqual(comparison < 0, a < b);

            #endregion

            #region Equality

            bool equality = comparison == 0;
            var equalityComparer = EqualityComparer<BrewVersion>.Default;

            Assert.AreEqual(equality, equalityComparer.Equals(a, b));

            if (equality && a is not null && b is not null)
            {
                Assert.AreEqual(
                    equalityComparer.GetHashCode(a),
                    equalityComparer.GetHashCode(b));
            }

            Assert.AreEqual(equality, a == b);
            Assert.AreNotEqual(equality, a != b);

            if (a is not null)
            {
                Assert.AreEqual(equality, a.Equals(b));
                Assert.AreEqual(equality, a.Equals((object?)b));
            }
            if (b is not null)
            {
                Assert.AreEqual(equality, b.Equals(a));
                Assert.AreEqual(equality, b.Equals((object?)a));
            }

            #endregion
        }
    }
}
