// Gapotchenko.Shields.Java
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2019

using Gapotchenko.FX.Math.Intervals;

namespace Gapotchenko.Shields.Java.Deployment;

/// <summary>
/// Provides deployment discovery services for Java.
/// </summary>
public static partial class JavaDeployment
{
    /// <summary>
    /// Enumerates setup instances of Java.
    /// The Java deployment lookup is optionally restricted by the given range of product versions.
    /// The instances are sorted by product version and edition.
    /// They are also filtered to include only ready to use instances.
    /// The newest and fullest versions come first.
    /// </summary>
    /// <param name="versions">The interval of Java versions to enumerate.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>Sequence of setup instances of Java.</returns>
    public static IEnumerable<IJavaSetupInstance> EnumerateSetupInstances(
        ValueInterval<Version> versions,
        JavaDiscoveryOptions options = default)
    {
        if (versions.IsEmpty)
            return [];

        return EnumerateSetupInstancesCore(JavaVersion.NaturalizeInterval(versions), options);
    }

    static IEnumerable<IJavaSetupInstance> EnumerateSetupInstancesCore(
        ValueInterval<Version> versions,
        JavaDiscoveryOptions options = default)
    {
        if ((options & JavaDiscoveryOptions.NoEnvironment) == 0)
        {
            var envInstance = JavaSetupInstanceEnv.TryCreate();
            if (envInstance != null && versions.Contains(envInstance.Version))
                yield return envInstance;
        }

        IEnumerable<IJavaSetupInstance> query;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            query = Pal.Windows.EnumerateSetupInstances(versions);
        else
            query = [];

        query = query
            .OrderByDescending(x => x.Version)
            .ThenByDescending(GetSetupInstanceScore);

        foreach (var i in query)
            yield return i;
    }

    static int GetSetupInstanceScore(IJavaSetupInstance instance)
    {
        int score = 0;
        string productId = instance.Product.Id;
        if (JavaProduct.IsSE(productId))
            score += 10;
        if (JavaProduct.IsSdk(productId))
            score += 1;
        return score;
    }
}
