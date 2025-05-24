// Gapotchenko.Shields.Java.Configuration
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2019

using Gapotchenko.FX.Text;
using System.Collections;

namespace Gapotchenko.Shields.Java.Configuration;

/// <summary>
/// A container holding a set of Java properties.
/// </summary>
public sealed class JavaProperties : IEnumerable<KeyValuePair<string, string>>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    readonly Dictionary<string, string> m_Map = new(StringComparer.Ordinal);

    /// <summary>
    /// Gets or sets the value of property with a specified name.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <returns>The property value.</returns>
    public string? this[string name]
    {
        get
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            m_Map.TryGetValue(name, out string? value);
            return value;
        }
        set
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (value == null)
                m_Map.Remove(name);
            else
                m_Map[name] = value;
        }
    }

    /// <summary>
    /// Clears all properties.
    /// </summary>
    public void Clear() => m_Map.Clear();

    /// <summary>
    /// Loads properties from a specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>The properties.</returns>
    static JavaProperties Load(Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        // TODO: Needs a better and more thorough implementation.

        var properties = new JavaProperties();

        using var tr = new StreamReader(stream);
        for (; ; )
        {
            string? line = tr.ReadLine();
            if (line == null)
                break;

            line = line.TrimStart();
            if (line.Length == 0)
                continue;

            if (line.StartsWith('!') || line.StartsWith('#'))
                continue;

            string[] parts = line.Split(['='], 2);
            if (parts.Length != 2)
                throw new InvalidDataException("Unexpected line structure in a Java property file.");
           
            string name = parts[0].TrimEnd();
            string value = parts[1].TrimStart();

            properties[name] = value;
        }

        return properties;
    }

    /// <summary>
    /// Loads properties from the file with a specified path.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <returns>The properties.</returns>
    public static JavaProperties Load(string filePath)
    {
        if (filePath == null)
            throw new ArgumentNullException(nameof(filePath));

        return Load(File.OpenRead(filePath));
    }

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => m_Map.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
