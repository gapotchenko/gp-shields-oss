// Gapotchenko.Shields.BusyBox
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.Diagnostics;
using Gapotchenko.FX.IO;

namespace Gapotchenko.Shields.BusyBox.Deployment;

partial class BusyBoxDeployment
{
    partial class Pal
    {
#if NET
        [SupportedOSPlatform("windows")]
#endif
        public static class Windows
        {
            public static IEnumerable<BusyBoxSetupDescriptor> EnumerateSetupDescriptors(BusyBoxDiscoveryOptions options)
            {
                return FripperyOrg.EnumerateSetupDescriptors(options);
            }

            public static IEnumerable<BusyBoxSetupDescriptor> EnumerateSetupDescriptors(string path)
            {
                return FripperyOrg.EnumerateSetupDescriptors(path);
            }

            public static bool TryResolveInstallationPath(
                in BusyBoxSetupDescriptor descriptor,
                [MaybeNullWhen(false)] out string installationPath,
                [MaybeNullWhen(false)] out string productPath)
            {
                installationPath = default;
                productPath = default;
                return false;
            }

            /// <summary>
            /// Provides support of "BusyBox for Windows" project.
            /// </summary>
            /// <remarks>
            /// More information: <see href="https://frippery.org/busybox/"/>
            /// </remarks>
            static class FripperyOrg
            {
                public static IEnumerable<BusyBoxSetupDescriptor> EnumerateSetupDescriptors(BusyBoxDiscoveryOptions options)
                {
                    if ((options & (BusyBoxDiscoveryOptions.NoPath | BusyBoxDiscoveryOptions.NoEnvironment)) == 0)
                        return EnumeratePath();
                    else
                        return [];

                    static IEnumerable<BusyBoxSetupDescriptor> EnumeratePath()
                    {
                        foreach (var productFileDescriptor in m_ProductFileDescriptors)
                        {
                            foreach (string path in CommandShell.Where(productFileDescriptor.Name))
                            {
                                if (TryGetSetupDescriptor(productFileDescriptor, path) is { } setupDescriptor)
                                    yield return setupDescriptor;
                            }
                        }
                    }
                }

                public static IEnumerable<BusyBoxSetupDescriptor> EnumerateSetupDescriptors(string path)
                {
                    if (Directory.Exists(path))
                    {
                        foreach (var productFileDescriptor in m_ProductFileDescriptors)
                        {
                            string filePath = Path.Combine(path, productFileDescriptor.Name);
                            if (File.Exists(filePath))
                            {
                                if (TryGetSetupDescriptor(productFileDescriptor, filePath) is { } setupDescriptor)
                                    yield return setupDescriptor;
                            }
                        }
                    }
                    else if (File.Exists(path))
                    {
                        string fileName = Path.GetFileName(path);
                        if (m_ProductFileDescriptors.FirstOrDefault(x => x.Name.Equals(fileName, FileSystem.PathComparison)) is { Name: [_, ..] } productFileDescriptor &&
                            TryGetSetupDescriptor(productFileDescriptor, path) is { } setupDescriptor)
                        {
                            yield return setupDescriptor;
                        }
                    }
                }

                static readonly IEnumerable<ProductFileDescriptor> m_ProductFileDescriptors =
                [
                    // In accordance with the project documentation and in historical order.
                    new("busybox.exe", Architecture.X86, BusyBoxSetupInstanceAttributes.None),
                    new("busybox64.exe", Architecture.X64, BusyBoxSetupInstanceAttributes.None),
                    new("busybox64u.exe", Architecture.X64, BusyBoxSetupInstanceAttributes.Unicode),
                    new("busybox64a.exe", Architecture.Arm64, BusyBoxSetupInstanceAttributes.Unicode)
                ];

                static BusyBoxSetupDescriptor? TryGetSetupDescriptor(in ProductFileDescriptor productFileDescriptor, string filePath)
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(filePath);

                    if (versionInfo.ProductName is not "busybox-w32")
                    {
                        // File was not produced by the project, so it impossible to make
                        // any assumptions about its characteristics.
                        return null;
                    }

                    string? productVersion = versionInfo.ProductVersion;
                    if (productVersion is null)
                        return null;

                    string? manufacturerVersion = null;
                    if (productVersion.IndexOf("-FRP-", StringComparison.Ordinal) is not -1 and var j)
                        manufacturerVersion = productVersion[(j + 1)..];
                    if (manufacturerVersion is null)
                        return null;

                    var version = new Version(versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductBuildPart);

                    return
                        new(GetRealPath(filePath))
                        {
                            Architecture = productFileDescriptor.Architecture,
                            Attributes = productFileDescriptor.Attributes | BusyBoxSetupInstanceAttributes.Path,
                            // Since the file version information has been retrieved, pass it along.
                            Version = version,
                            ManufacturerVersion = manufacturerVersion
                        };
                }

                readonly record struct ProductFileDescriptor(
                    string Name,
                    Architecture Architecture,
                    BusyBoxSetupInstanceAttributes Attributes);
            }
        }
    }
}
