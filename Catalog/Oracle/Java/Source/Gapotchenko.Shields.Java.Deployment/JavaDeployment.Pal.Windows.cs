// Gapotchenko.Shields.Java
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2019

using Gapotchenko.FX.Math.Intervals;
using Gapotchenko.FX.Text;
using Microsoft.Win32;

namespace Gapotchenko.Shields.Java.Deployment;

partial class JavaDeployment
{
    static partial class Pal
    {
#if NET
        [SupportedOSPlatform("windows")]
#endif
        public static class Windows
        {
            public static IEnumerable<IJavaSetupInstance> EnumerateSetupInstances(Interval<Version> versions)
            {
                return EnumerateSetupInstancesFromRegistry(versions);
            }

            static IEnumerable<IJavaSetupInstance> EnumerateSetupInstancesFromRegistry(Interval<Version> versions)
            {
                using var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                using var javaSoftKey = hklm?.OpenSubKey(@"SOFTWARE\JavaSoft");
                if (javaSoftKey == null)
                    yield break;

                using (var jdkKey = javaSoftKey.OpenSubKey("JDK"))
                {
                    if (jdkKey != null)
                    {
                        foreach (string jdkVersionKeyName in jdkKey.GetSubKeyNames())
                        {
                            if (!Version.TryParse(jdkVersionKeyName, out var version))
                                continue;

                            if (!versions.Contains(version))
                                continue;

                            using var jdkVersionKey = jdkKey.OpenSubKey(jdkVersionKeyName);
                            if (jdkVersionKey == null)
                                continue;

                            string? homePath = jdkVersionKey.GetValue("JavaHome") as string;

                            var instance = JavaSetupInstanceFS.TryCreate(homePath, JavaProduct.IDs.SE.Sdk);
                            if (instance != null)
                                yield return instance;
                        }
                    }
                }

                using (var key = javaSoftKey.OpenSubKey("Java Development Kit"))
                {
                    if (key != null)
                    {
                        foreach (var i in EnumerateSetupInstancesFromRegistryKey_v1_8(key, versions, JavaProduct.IDs.SE.Sdk))
                            yield return i;
                    }
                }

                using (var key = javaSoftKey.OpenSubKey("Java Runtime Environment"))
                {
                    if (key != null)
                    {
                        foreach (var i in EnumerateSetupInstancesFromRegistryKey_v1_8(key, versions, JavaProduct.IDs.SE.Runtime))
                            yield return i;
                    }
                }
            }

            static IEnumerable<IJavaSetupInstance> EnumerateSetupInstancesFromRegistryKey_v1_8(RegistryKey jdkKey, Interval<Version> versions, string productIDHint)
            {
                foreach (string jdkVersionKeyName in jdkKey.GetSubKeyNames())
                {
                    if (!(jdkVersionKeyName.Contains('_') && Version.TryParse(jdkVersionKeyName.Replace('_', '.'), out var version)))
                        continue;

                    if (!versions.Contains(version))
                        continue;

                    using var jdkVersionKey = jdkKey.OpenSubKey(jdkVersionKeyName);
                    if (jdkVersionKey == null)
                        continue;

                    string? homePath = jdkVersionKey.GetValue("JavaHome") as string;

                    var instance = JavaSetupInstanceFS.TryCreate(homePath, productIDHint);
                    if (instance != null)
                        yield return instance;
                }
            }
        }
    }
}

