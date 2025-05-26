// Gapotchenko.Shields.Java
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2019

namespace Gapotchenko.Shields.Java.Deployment;

partial class JavaProduct
{
    const string RuntimeSuffix = ".Runtime";
    const string SdkSuffix = ".SDK";

    /// <summary>
    /// Represents well-known product identifier of Java.
    /// </summary>
    public static class IDs
    {
        /// <summary>
        /// A meta product identifier for Java.
        /// It does not occur in reality under normal conditions.
        /// It can be used to represent a catch-all identity for various Java products.
        /// It also serves as a prefix to all other Java product identifier.
        /// </summary>
        public const string Meta = "Java.Product";

        /// <summary>
        /// Represents well-known product identifier for Java SE.
        /// </summary>
        public static class SE
        {
            /// <summary>
            /// A meta product identifier for Java SE.
            /// It does not occur in reality under normal conditions.
            /// It can be used to represent a catch-all identity for various Java SE products.
            /// It also serves as a prefix to all other Java SE product identifier.
            /// </summary>
            public const string Meta = IDs.Meta + ".SE";

            /// <summary>
            /// Product identifier for JRE (Java SE Runtime).
            /// </summary>
            public const string Runtime = Meta + RuntimeSuffix;

            /// <summary>
            /// Product identifier for JDK (Java SE SDK).
            /// </summary>
            public const string Sdk = Meta + SdkSuffix;
        }
    }
}
