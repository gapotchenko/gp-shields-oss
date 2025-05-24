// Gapotchenko.Shields.Microsoft.PowerShell.Script
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.FX.Diagnostics;
using Gapotchenko.Shields.Microsoft.PowerShell.Deployment;
using System.Text;

namespace Gapotchenko.Shields.Microsoft.PowerShell.Scripting;

/// <summary>
/// Provides functionality for PowerShell scripts creation and execution.
/// </summary>
public static class PowerShellScript
{
    /// <summary>
    /// Executes the specified PowerShell script.
    /// </summary>
    /// <param name="setupInstance">The PowerShell setup instance to use for script execution.</param>
    /// <param name="script">The script.</param>
    /// <param name="options">The options.</param>
    /// <returns>The script exit code.</returns>
    public static int Execute(IPowerShellSetupInstance setupInstance, string script, PowerShellScriptExecutionOptions? options = null)
    {
        if (setupInstance is null)
            throw new ArgumentNullException(nameof(setupInstance));
        if (script is null)
            throw new ArgumentNullException(nameof(script));

        using var process = CreateExecutionProcess(setupInstance, options);

        process.Start();

        var stdin = process.StandardInput;

        // Use Base64-encoded UTF-8 strings to avoid issues with legacy console encodings.
        stdin.Write(Convert.ToBase64String(Encoding.UTF8.GetBytes(script)));

        // EOF
        stdin.Close();

        process.WaitForExit();

        return process.ExitCode;
    }

    /// <summary>
    /// Asynchronously executes the specified PowerShell script.
    /// </summary>
    /// <param name="setupInstance">The PowerShell setup instance to use for script execution.</param>
    /// <param name="script">The script.</param>
    /// <param name="options">The options.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The script exit code.</returns>
    public static async Task<int> ExecuteAsync(
        IPowerShellSetupInstance setupInstance,
        string script,
        PowerShellScriptExecutionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (setupInstance is null)
            throw new ArgumentNullException(nameof(setupInstance));
        if (script is null)
            throw new ArgumentNullException(nameof(script));

        using var process = CreateExecutionProcess(setupInstance, options);

        process.Start();

        var stdin = process.StandardInput;

        // Use Base64-encoded UTF-8 strings to avoid issues with legacy console encodings.
        await stdin
            .WriteAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(script)))
            .ConfigureAwait(false);

        // Signal an end of file.
        // We do an async flush because there is no async close.
        await stdin
#if NET8_0_OR_GREATER
            .FlushAsync(cancellationToken)
#else
            .FlushAsync()
#endif
            .ConfigureAwait(false);
        stdin.Close();

        await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);

        return process.ExitCode;
    }

    static Process CreateExecutionProcess(IPowerShellSetupInstance setupInstance, PowerShellScriptExecutionOptions? options)
    {
        bool interactive = options?.Interactive ?? false;
        if (interactive)
            throw new NotImplementedException("Interactive PowerShell script execution is not implemented.");

        var process = new Process();

        var psi = process.StartInfo;
        psi.FileName = setupInstance.ResolvePath(setupInstance.ProductPath);
        psi.Arguments = "-noprofile -noninteractive -command \"[Text.Encoding]::UTF8.GetString([Convert]::FromBase64String($input)) | iex\"";
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;
        psi.RedirectStandardInput = true;

        return process;
    }

    /// <summary>
    /// Gets a quoted PowerShell script string representation of the specified value.
    /// </summary>
    /// <param name="value">The value to quote.</param>
    /// <returns>A quoted string representation of the specified <paramref name="value"/>.</returns>
    public static string QuoteString(string? value)
    {
        if (value is null)
            return Null;
        else
        {
            var sb = new StringBuilder();
            sb.AppendLine("@\"");
            foreach (char ch in value)
            {
                if (ch is '"' or '$' or '`')
                    sb.Append('`');
                sb.Append(ch);
            }
            return sb
                .AppendLine()
                .Append("\"@")
                .ToString();
        }
    }

    /// <summary>
    /// Gets a PowerShell script expression representing a null value (<c>$null</c>).
    /// </summary>
    public static string Null => "$null";
}
