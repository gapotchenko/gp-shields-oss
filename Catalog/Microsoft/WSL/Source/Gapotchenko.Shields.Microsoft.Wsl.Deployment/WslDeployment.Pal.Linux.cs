// Gapotchenko.Shields.Microsoft.Wsl
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
#define TFF_PROCESS_ARGUMENTLIST
#endif

using Gapotchenko.FX.Collections.Generic;
using Gapotchenko.FX.Diagnostics;
using Gapotchenko.FX.Math.Intervals;
using Gapotchenko.Shields.Microsoft.Wsl.Deployment.Utils;

namespace Gapotchenko.Shields.Microsoft.Wsl.Deployment;

partial class WslDeployment
{
    partial class Pal
    {
#if NET
        [SupportedOSPlatform("linux")]
#endif
        public static class Linux
        {
            public static IEnumerable<IWslSetupInstance> EnumerateSetupInstances(Interval<Version> versions)
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WSL_INTEROP")) &&
                    !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WSL_DISTRO_NAME")))
                {
                    IWslSetupInstance? setupInstance;
                    try
                    {
                        setupInstance = TryGetSetupInstance_V2Plus(versions);
                    }
                    catch
                    {
                        setupInstance = null;
                    }

                    if (setupInstance != null)
                        yield return setupInstance;
                }
            }

            static IWslSetupInstance? TryGetSetupInstance_V2Plus(Interval<Version> versions)
            {
                // Make a callback to Windows from Linux to query WSL data from Windows registry.

                var psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
#if !(NETCOREAPP || NETSTANDARD2_1_OR_GREATER)
                    UseShellExecute = false
#endif
                };

                var args =
#if TFF_PROCESS_ARGUMENTLIST
                    psi.ArgumentList;
#else
                    new List<string>();
#endif

                args.Add("/c");
                args.Add(@"reg query HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss\MSI /reg:64");

#if !TFF_PROCESS_ARGUMENTLIST
                psi.Arguments = CommandLine.Build(args);
#endif

                var output = new StringWriter();
                int exitCode = ProcessUtil.Execute(psi, output);
                if (exitCode != 0)
                    return null;

                var registry = RegUtil.ParseQueryOutput(new StringReader(output.ToString()));

                // Interpret the WSL registry data.

                if (registry.GetValueOrDefault("Version") is not string versionString)
                    return null;
                if (!Version.TryParse(versionString, out var version))
                    return null;
                if (!versions.Contains(version))
                    return null;

                string? installLocation = TranslateFilePath(registry.GetValueOrDefault("InstallLocation") as string);
                if (!Directory.Exists(installLocation))
                    return null;

                return WslSetupInstance.TryCreate(installLocation, version);
            }

            [return: NotNullIfNotNull(nameof(path))]
            static string? TranslateFilePath(string? path)
            {
                if (string.IsNullOrEmpty(path))
                    return path;

                var psi = new ProcessStartInfo
                {
                    FileName = "wslpath",
#if !(NETCOREAPP || NETSTANDARD2_1_OR_GREATER)
                    UseShellExecute = false
#endif
                };

#if TFF_PROCESS_ARGUMENTLIST
                psi.ArgumentList.Add(path);
#else
                psi.Arguments = CommandLine.Build(path);
#endif

                var output = new StringWriter();

                int exitCode = ProcessUtil.Execute(psi, output);
                if (exitCode != 0)
                    return path;

                return output.ToString();
            }
        }
    }
}
