// Gapotchenko.Shields.Microsoft.Wsl
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Data.Encoding;
using System.Text.RegularExpressions;

namespace Gapotchenko.Shields.Microsoft.Wsl.Deployment.Utils;

/// <summary>
/// Provides utilities for the <c>reg</c> command available on Windows.
/// </summary>
static class RegUtil
{
    /// <summary>
    /// Parses the output of a <c>reg query &lt;keyname&gt;</c> command.
    /// </summary>
    /// <param name="reader">The text reader.</param>
    /// <returns>The parsed dictionary of values.</returns>
    public static IReadOnlyDictionary<string, object> ParseQueryOutput(TextReader reader)
    {
        var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        var regex = new Regex(
            @"^\s*(?<name>.+?)\s+(?<type>REG_\w+)\s+(?<data>.*)\s*?$",
            RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant);

        while (reader.ReadLine() is { } line)
        {
            var match = regex.Match(line);
            if (!match.Success)
                continue;

            string name = match.Groups["name"].Value;
            string type = match.Groups["type"].Value;
            string data = match.Groups["data"].Value;

            object parsedValue =
                type switch
                {
                    "REG_SZ" or "REG_EXPAND_SZ" => data,
                    "REG_DWORD" => ParseDword(data),
                    "REG_QWORD" => ParseQword(data),
                    "REG_MULTI_SZ" => data.Split(['\0'], StringSplitOptions.RemoveEmptyEntries),
                    "REG_BINARY" => ParseHexBytes(data),
                    _ => data
                };

            result[name] = parsedValue;
        }

        return result;

        static int ParseDword(string data)
        {
            return data.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                ? Convert.ToInt32(data, 16)
                : int.Parse(data);
        }

        static long ParseQword(string data)
        {
            return data.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                ? Convert.ToInt64(data, 16)
                : long.Parse(data);
        }

        static byte[] ParseHexBytes(string hex) => Base16.GetBytes(hex);
    }
}
