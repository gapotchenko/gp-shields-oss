// Gapotchenko.Shields.Homebrew
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using System.Text;
using Gapotchenko.FX;
using Gapotchenko.FX.Linq;
using Gapotchenko.FX.Math.Intervals;
using Gapotchenko.Shields.Homebrew.Deployment.Utils;

namespace Gapotchenko.Shields.Homebrew.Deployment;

static class BrewSetupInstance
{
    internal static IBrewSetupInstance? TryCreate(
        string installationPath,
        IEnumerable<BrewSetupDescriptor> descriptors,
        Interval<Version> versions)
    {
        if (!ResemblesValidInstallation(installationPath))
            return null;

        descriptors = descriptors.Memoize();

        string? productFileName =
            descriptors
                .Select(x => x.ProductFileName)
                .FirstOrDefault(x => x != null)
            ?? ProductUtil.GetProductFileName();

        string productPath = Path.Combine("bin", productFileName);

        string brewPath = Path.Combine(installationPath, productPath);
        if (!File.Exists(brewPath))
            return null;

        var version = Lazy.Create(() => GetVersion(brewPath));
        if (!versions.IsInfinite && !versions.Contains(version.Value))
            return null;
        
        return new BrewSetupInstanceImpl(
            installationPath, 
            productPath, 
            version,
            descriptors.Select(x => x.Attributes).Aggregate((a, b) => a | b),
            descriptors.Select(x => x.CellarPath).FirstOrDefault(x => x != null),
            descriptors.Select(x => x.RepositoryPath).FirstOrDefault(x => x != null));
    }

    static bool ResemblesValidInstallation(string installationPath)
    {
        if (!Directory.Exists(Path.Combine(installationPath, "var", "homebrew")))
            return false;
        return true;
    }
    
    static Version GetVersion(string brewPath)
    {
        // There is no easy way to get Homebrew version except asking the app itself.

        var processStartInfo = new ProcessStartInfo(brewPath)
        {
#if !NETCOREAPP
            UseShellExecute = false,
#endif
            WindowStyle = ProcessWindowStyle.Hidden,
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
            throw new BrewDeploymentException(
                string.Format("Unable to run '{0}'.", processStartInfo.FileName),
                e);
        }

        if (exitCode != 0)
        {
            throw new BrewDeploymentException(
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
            const string prefix = "Homebrew ";
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

        return version ?? throw new BrewDeploymentException("Cannot determine Homebrew version.");
    }
}
