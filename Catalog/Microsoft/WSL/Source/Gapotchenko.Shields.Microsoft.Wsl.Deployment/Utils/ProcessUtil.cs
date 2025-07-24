// Gapotchenko.Shields.Microsoft.Wsl
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

namespace Gapotchenko.Shields.Microsoft.Wsl.Deployment.Utils;

static class ProcessUtil
{
    public static int Execute(ProcessStartInfo psi, TextWriter output)
    {
        psi.CreateNoWindow = true;
        psi.RedirectStandardOutput = true;

        using var process =
            Process.Start(psi) ??
            throw new InvalidOperationException(string.Format("Cannot start '{0}' process.", psi.FileName));

        bool hasOutput = false;

        void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data is { } data)
            {
                if (hasOutput)
                    output.WriteLine();
                output.Write(data);
                hasOutput = true;
            }
        }

        process.OutputDataReceived += Process_OutputDataReceived;
        process.BeginOutputReadLine();

        process.WaitForExit();

        return process.ExitCode;
    }
}
