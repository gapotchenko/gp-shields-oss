// Gapotchenko.Shields.Git
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX;
using Gapotchenko.FX.Linq;
using Gapotchenko.FX.Math.Intervals;
using System.Text;

namespace Gapotchenko.Shields.Git.Deployment;

static class GitSetupInstance
{
    internal static IGitSetupInstance? TryCreate(
        IEnumerable<GitSetupDescriptor> descriptors,
        Interval<Version> versions)
    {
        descriptors = descriptors.Memoize();

        // ------------------------------------------------------------------

        string? installationPath = null;
        string? productPath = null;

        foreach (var descriptor in descriptors)
        {
            if (descriptor.InstallationPath is { } path)
            {
                installationPath = path;
                productPath = descriptor.ProductPath;
                break;
            }
        }

        productPath ??= descriptors.First().ProductPath;

        if (installationPath is null)
        {
            installationPath = TryDetermineInstallationPath(productPath);
            if (installationPath is null)
                return null;
        }

        // ------------------------------------------------------------------

        string gitPath = Path.Combine(installationPath, productPath);
        if (!File.Exists(gitPath))
            return null;

        Lazy<Version> version =
            descriptors.Select(x => x.Version).FirstOrDefault(x => x != null) is { } determinedVersion
                ?
#if NETCOREAPP
                new(determinedVersion)
#else
                new(() => determinedVersion)
#endif
                : new(() => GetVersion(gitPath));

        if (!versions.IsInfinite && !versions.Contains(version.Value))
            return null;

        return new GitSetupInstanceImpl(
            installationPath,
            productPath,
            version,
            descriptors.Select(x => x.Attributes).Aggregate((a, b) => a | b));
    }

    static string? TryDetermineInstallationPath(string productPath)
    {
        // TODO

        return null;
    }

    static Version GetVersion(string gitPath)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(gitPath);
            return new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart);
        }
        else
        {
            // There is no easy way to get Git version on Unix except asking the app itself.

            var processStartInfo = new ProcessStartInfo(gitPath)
            {
#if !NETCOREAPP
                UseShellExecute = false,
#endif
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

#if NETCOREAPP
            processStartInfo.ArgumentList.Add("--version");
#else
            processStartInfo.Arguments = "--version";
#endif

            int exitCode;
            StringBuilder output;

            try
            {
                using var process =
                    Process.Start(processStartInfo) ??
                    throw new InvalidOperationException("Process cannot be started.");

                output = new();
                process.OutputDataReceived += ProcessOutputDataReceived;
                process.ErrorDataReceived += ProcessOutputDataReceived;

                void ProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
                {
                    if (e.Data is { } data)
                    {
                        lock (output)
                            output.AppendLine(data);
                    }
                }

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                exitCode = process.ExitCode;
            }
            catch (Exception e) when (!e.IsControlFlowException())
            {
                throw new GitDeploymentException(
                    string.Format("Unable to run '{0}'.", processStartInfo.FileName),
                    e);
            }

            if (exitCode != 0)
            {
                throw new GitDeploymentException(
                    // Format the process output.
                    Empty.Nullify(output.ToString().Trim())
                    // Or use a fallback message in case of the absent process output.
                    ?? string.Format(
                        "Process '{0}' failed with exit code {1}.",
                        processStartInfo.FileName,
                        exitCode));
            }

            Version? version = null;

            var reader = new StringReader(output.ToString());
            while (reader.ReadLine() is { } line)
            {
                const string prefix = "git version ";
                if (line.StartsWith(prefix, StringComparison.Ordinal))
                {
                    version = Version.Parse(
                        line
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                            .AsSpan()
#endif
                            [prefix.Length..]);
                    break;
                }
            }

            return version ?? throw new GitDeploymentException("Cannot determine Git version.");
        }
    }
}
