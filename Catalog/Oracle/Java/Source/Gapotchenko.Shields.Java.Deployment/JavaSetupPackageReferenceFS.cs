// Gapotchenko.Shields.Java
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2019

using Gapotchenko.FX.Threading;
using Gapotchenko.Shields.Java.Configuration;

namespace Gapotchenko.Shields.Java.Deployment;

sealed class JavaSetupPackageReferenceFS : IJavaSetupPackageReference
{
    public JavaSetupPackageReferenceFS(string homePath, string? productIdHint, Lazy<JavaProperties?> releaseManifest)
    {
        m_HomePath = homePath;
        m_ProductIdHint = productIdHint;
        m_ReleaseManifest = releaseManifest;

        m_Id = new(GetIdCore);
    }

    readonly string m_HomePath;
    readonly string? m_ProductIdHint;
    readonly Lazy<JavaProperties?> m_ReleaseManifest;

    public string Id => m_Id.Value;

    readonly EvaluateOnce<string> m_Id;

    string GetIdCore()
    {
        var releaseManifest =
            m_ReleaseManifest.Value ??
            throw new InvalidDataException("Java instance has no release manifest.");

        string? s;

        s = releaseManifest["MODULES"];
        if (s != null)
        {
            s = s.Trim('"');

            var h = new HashSet<string>(s.Split(' '), StringComparer.Ordinal);

            if (h.Contains("jdk.compiler"))
                return JavaProduct.IDs.SE.Sdk;
            else
                return JavaProduct.IDs.SE.Runtime;
        }

        if (Directory.EnumerateFiles(Path.Combine(m_HomePath, "bin"), "javac.*").Any())
            return JavaProduct.IDs.SE.Sdk;
        if (Directory.EnumerateFiles(Path.Combine(m_HomePath, "bin"), "java.*").Any())
            return JavaProduct.IDs.SE.Runtime;

        if (m_ProductIdHint != null)
            return m_ProductIdHint;

        throw new NotSupportedException("Cannot determine Java product identifier.");
    }
}
