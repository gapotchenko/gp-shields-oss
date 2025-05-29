// Gapotchenko.Shields.Xdg.Directories
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

using Gapotchenko.Shields.Xdg.Directories.User.Pal;

namespace Gapotchenko.Shields.Xdg.Directories.User;

partial struct XdgUserDirectory
{
    /// <summary>
    /// Gets the directory value.
    /// If the directory is unknown, throws an exception.
    /// </summary>
    /// <returns>The XDG directory value.</returns>
    /// <exception cref="InvalidOperationException">The XDG user directory is unknown.</exception>
    public string Value =>
        ValueOrDefault ??
        throw new InvalidOperationException("The XDG user directory is unknown.");

    /// <summary>
    /// Tries to get the directory value.
    /// </summary>
    /// <returns>The XDG directory value, or <see langword="null"/> if the directory is unknown.</returns>
    public string? ValueOrDefault
    {
        get
        {
            string name = Name;
            if (name.Length == 0)
                return null;

            var cache = m_ValueCache;
            lock (cache)
            {
                if (cache.TryGetValue(name, out string? value))
                    return value;

                if (!IsKnown)
                    return null;

                value = TryGetValueCore(name);
                cache.Add(name, value);
                return value;
            }
        }
    }

    /// <summary>
    /// Invalidates cached XDG user directory values.
    /// </summary>
    public static void Refresh()
    {
        ClearCache(m_ValueCache);
    }

    static void ClearCache<TKey, TValue>(Dictionary<TKey, TValue> cache) where TKey : notnull
    {
        lock (cache)
            cache.Clear();
    }

    static readonly Dictionary<string, string?> m_ValueCache = new(StringComparer.Ordinal);

    static string? TryGetValueCore(string name) =>
        Empty.Nullify(Environment.GetEnvironmentVariable(name)) ??
        TryGetDefaultValue(name);

    static string? TryGetDefaultValue(string name)
    {
        var pal = PalServices.Adapter;

        string? value =
            name switch
            {
                "XDG_DESKTOP_DIR" => pal.GetDesktopDirectory(),
                "XDG_DOWNLOAD_DIR" => pal.GetDownloadsDirectory(),
                "XDG_DOCUMENTS_DIR" => pal.GetDocumentsDirectory(),
                "XDG_MUSIC_DIR" => pal.GetMusicDirectory(),
                "XDG_PICTURES_DIR" => pal.GetPicturesDirectory(),
                "XDG_VIDEOS_DIR" => pal.GetVideosDirectory(),
                "XDG_TEMPLATES_DIR" => pal.GetTemplatesDirectory(),
                "XDG_PUBLICSHARE_DIR" => pal.GetPublicDirectory(),
                _ => null
            };

        Debug.Assert(value != null, $"{name} base directory support is not implemented.");
        return value;
    }
}
