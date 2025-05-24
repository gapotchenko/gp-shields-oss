// Gapotchenko.Shields.Xdg.Directories.Base
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.Shields.Xdg.Directories.Base.Pal;

namespace Gapotchenko.Shields.Xdg.Directories.Base;

partial struct XdgBaseDirectory
{
    /// <summary>
    /// Invalidates cached XDG base directory values.
    /// </summary>
    public static void Refresh()
    {
        ClearCache(m_ValueCache);
        ClearCache(m_ValuesCache);
    }

    static void ClearCache<TKey, TValue>(Dictionary<TKey, TValue> cache) where TKey : notnull
    {
        lock (cache)
            cache.Clear();
    }

    /// <summary>
    /// Gets the directory values.
    /// If the directory is unknown, throws an exception.
    /// </summary>
    /// <returns>The XDG directory values.</returns>
    /// <exception cref="InvalidOperationException">The XDG base directory is unknown.</exception>
    public IReadOnlyList<string> Values =>
        ValuesOrDefault ??
        throw new InvalidOperationException("The XDG base directory is unknown.");

    /// <summary>
    /// Tries to get the directory values.
    /// </summary>
    /// <returns>The XDG directory values, or <see langword="null"/> if the directory is unknown.</returns>
    public IReadOnlyList<string>? ValuesOrDefault
    {
        get
        {
            var name = Name;
            if (name.Length == 0)
                return null;

            var cache = m_ValuesCache;
            lock (cache)
            {
                if (cache.TryGetValue(name, out var value))
                    return value;

                if (!IsKnown)
                    return null;

                value = SplitValues(TryGetValueCore(name));
                cache.Add(name, value);
                return value;
            }

            [return: NotNullIfNotNull(nameof(value))]
            static IReadOnlyList<string>? SplitValues(string? value) =>
                value?.Split(new[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    static readonly Dictionary<string, IReadOnlyList<string>?> m_ValuesCache = new(StringComparer.Ordinal);

    /// <summary>
    /// Gets the directory value.
    /// If the directory is unknown, throws an exception.
    /// </summary>
    /// <returns>The XDG directory value.</returns>
    /// <exception cref="InvalidOperationException">The XDG base directory is unknown.</exception>
    public string Value =>
        ValueOrDefault ??
        throw new InvalidOperationException("The XDG base directory is unknown.");

    /// <summary>
    /// Tries to get the directory value.
    /// </summary>
    /// <returns>The XDG directory value, or <see langword="null"/> if the directory is unknown.</returns>
    public string? ValueOrDefault
    {
        get
        {
            var name = Name;
            if (name.Length == 0)
                return null;

            var cache = m_ValueCache;
            lock (cache)
            {
                if (cache.TryGetValue(name, out var value))
                    return value;

                if (!IsKnown)
                    return null;

                value = TryGetValueCore(name);
                cache.Add(name, value);
                return value;
            }
        }
    }

    static readonly Dictionary<string, string?> m_ValueCache = new(StringComparer.Ordinal);

    static string? TryGetValueCore(string name) =>
        Empty.Nullify(Environment.GetEnvironmentVariable(name)) ??
        TryGetDefaultValue(name);

    static string? TryGetDefaultValue(string name)
    {
        var pal = PalServices.Adapter;

        var value =
            name switch
            {
                "XDG_DATA_HOME" => pal.GetDataHome(),
                "XDG_DATA_DIRS" => pal.GetDataDirectories(),
                "XDG_CONFIG_HOME" => pal.GetConfigurationHome(),
                "XDG_CONFIG_DIRS" => pal.GetConfigurationDirectories(),
                "XDG_STATE_HOME" => pal.GetStateHome(),
                "XDG_CACHE_HOME" => pal.GetCacheHome(),
                "XDG_RUNTIME_DIR" => pal.GetRuntimeDirectory(),
                _ => null
            };

        Debug.Assert(value != null, $"{name} base directory support is not implemented.");
        return value;
    }
}
