// Gapotchenko.Shields.Java
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2019

namespace Gapotchenko.Shields.Java.Deployment;

/// <summary>
/// Provides Java product identity operations.
/// </summary>
public static partial class JavaProduct
{
    /// <summary>
    /// Determines whether the given product identifier corresponds to Java SE (Standard Edition).
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <returns>
    /// <c>true</c> when the given product identifier corresponds to Java SE (Standard Edition);
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool IsSE(string productId) => IsPrefixedWith(productId, IDs.SE.Meta);

    /// <summary>
    /// Determines whether the given product identifier corresponds to a Java Runtime.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <returns>
    /// <c>true</c> when the given product identifier corresponds to a Java Runtime;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool IsRuntime(string productId) => IsSuffixedWith(productId, RuntimeSuffix);

    /// <summary>
    /// Determines whether the given product identifier corresponds to a Java SDK.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <returns>
    /// <c>true</c> when the given product identifier corresponds to a Java SDK;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool IsSdk(string productId) => IsSuffixedWith(productId, SdkSuffix);

    static bool IsPrefixedWith(string id, string prefix)
    {
        if (id == null)
            return false;

        if (!id.StartsWith(prefix, StringComparison.Ordinal))
            return false;

        int prefixLength = prefix.Length;
        if (id.Length != prefixLength)
        {
            if (id[prefixLength] != '.')
                return false;
        }

        return true;
    }

    static bool IsSuffixedWith(string id, string suffix)
    {
        if (id == null)
            return false;
        else
            return id.EndsWith(suffix, StringComparison.Ordinal);
    }
}
