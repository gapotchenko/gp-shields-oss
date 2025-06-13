// Gapotchenko.Shields.MSys2
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Memory;

namespace Gapotchenko.Shields.MSys2.Deployment.Utils;

static class IniUtil
{
    public static IEnumerable<(string? Section, string Key, string Value)> Read(TextReader reader)
    {
        // A primitive implementation, doesn't handle quotes, escapes, etc. properly.

        string? section = null;

        for (; ; )
        {
            var line = reader.ReadLine().AsSpan();
            if (line == null)
                break; // EOF
            line = line.Trim();
            if (line.IsEmpty)
                continue;

            char ch0 = line[0];
            if (ch0 == ';')
            {
                // A comment.
            }
            else if (ch0 == '[')
            {
                // A section.

                if (line.EndsWith(']'))
                    section = line[1..^1].ToString();

                continue;
            }
            else
            {
                // A key value.

                int j = line.IndexOf('=');
                if (j == -1)
                    continue;

                var key = line[..j].TrimEnd();
                if (key.IsEmpty)
                    continue;

                var value = line[(j + 1)..].TrimStart();

                yield return new(section, key.ToString(), value.ToString());
            }
        }
    }
}
