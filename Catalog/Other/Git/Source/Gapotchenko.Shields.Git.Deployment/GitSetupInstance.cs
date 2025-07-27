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
using System.Text.RegularExpressions;

namespace Gapotchenko.Shields.Git.Deployment;

/// <summary>
/// Provides static methods for working with Git setup instances.
/// </summary>
public static class GitSetupInstance
{
    /// <summary>
    /// Opens an Git setup instance at the specified directory path.
    /// </summary>
    /// <param name="directoryPath">The directory path to open an Git setup instance at.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>The opened Git setup instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="directoryPath"/> is <see langword="null"/>.</exception>
    /// <exception cref="IOException">An I/O error occurred while accessing the file system.</exception>
    /// <exception cref="GitDeploymentException">Cannot open an Git setup instance at the specified path.</exception>
    public static IGitSetupInstance Open(string directoryPath, GitDiscoveryOptions options = default) =>
        TryOpen(directoryPath, options) ??
        throw new GitDeploymentException("Cannot open a Git setup instance at the specified path.");

    /// <summary>
    /// Tries to open an Git setup instance at the specified directory path of the file system view.
    /// </summary>
    /// <param name="directoryPath">The directory path to open an Git setup instance at.</param>
    /// <param name="options">The discovery options.</param>
    /// <returns>
    /// The opened Git setup instance
    /// or <see langword="null"/> if <paramref name="directoryPath"/> does not contain it.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="directoryPath"/> is <see langword="null"/>.</exception>
    /// <exception cref="IOException">An I/O error occurred while accessing the file system.</exception>
    public static IGitSetupInstance? TryOpen(string directoryPath, GitDiscoveryOptions options = default)
    {
        ArgumentNullException.ThrowIfNull(directoryPath);

        if (File.Exists(Path.Combine(directoryPath, "git-cmd.exe")))
        {
            // Git for Windows

            string productPath = @"cmd\git.exe";

            string gitPath = Path.Combine(directoryPath, productPath);
            if (!File.Exists(gitPath))
                return null;

            return new GitSetupInstanceImpl(
                directoryPath,
                productPath,
                new(() => GetVersion(gitPath)),
                GitSetupInstanceAttributes.None);
        }

        return null;
    }

    internal static IGitSetupInstance? TryCreate(
        string installationPath,
        IEnumerable<GitSetupDescriptor> descriptors,
        Interval<Version> versions)
    {
        descriptors = descriptors.Memoize();

        string productPath = descriptors.First().ProductPath;

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
                    // Git version resembles a semantic version but it is not.
                    // For example, "2.50.1.windows.1" is not a valid semantic version.
                    // Do the best effort by extracting the sure parts of the version.
                    var match = Regex.Match(line[prefix.Length..], @"\d+\.\d+\.\d+", RegexOptions.CultureInvariant);
                    version = Version.Parse(match.Value);
                    break;
                }
            }

            return version ?? throw new GitDeploymentException("Cannot determine Git version.");
        }
    }
}
