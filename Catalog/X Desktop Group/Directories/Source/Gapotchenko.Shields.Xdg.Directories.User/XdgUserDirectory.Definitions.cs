// Gapotchenko.Shields.Xdg.Directories.User
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.User;

partial struct XdgUserDirectory
{
    /// <summary>
    /// <para>
    /// Defines the directory relative to which user-specific desktop files should be stored.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_DESKTOP_DIR</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default value is used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>Desktop</c> known folder, usually pointing at <c>%USERPROFILE%\Desktop</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Desktop</c>
    /// </item>
    /// <item>
    /// On Unix: <c>~/Desktop</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgUserDirectory Desktop { get; } = new("XDG_DESKTOP_DIR");

    /// <summary>
    /// <para>
    /// Defines the directory relative to which user-specific download files should be stored.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_DOWNLOAD_DIR</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default value is used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>Downloads</c> known folder, usually pointing at <c>%USERPROFILE%\Downloads</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Downloads</c>
    /// </item>
    /// <item>
    /// On Unix: <c>~/Downloads</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgUserDirectory Downloads { get; } = new("XDG_DOWNLOAD_DIR");

    /// <summary>
    /// <para>
    /// Defines the directory relative to which user-specific document files should be stored.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_DOCUMENTS_DIR</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default value is used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>Documents</c> known folder, usually pointing at <c>%USERPROFILE%\Documents</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Documents</c>
    /// </item>
    /// <item>
    /// On Unix: <c>~/Documents</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgUserDirectory Documents { get; } = new("XDG_DOCUMENTS_DIR");

    /// <summary>
    /// <para>
    /// Defines the directory relative to which user-specific music files should be stored.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_MUSIC_DIR</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default value is used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>Music</c> known folder, usually pointing at <c>%USERPROFILE%\Music</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Music</c>
    /// </item>
    /// <item>
    /// On Unix: <c>~/Music</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgUserDirectory Music { get; } = new("XDG_MUSIC_DIR");

    /// <summary>
    /// <para>
    /// Defines the directory relative to which user-specific picture files should be stored.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_PICTURES_DIR</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default value is used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>Pictures</c> known folder, usually pointing at <c>%USERPROFILE%\Pictures</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Pictures</c>
    /// </item>
    /// <item>
    /// On Unix: <c>~/Pictures</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgUserDirectory Pictures { get; } = new("XDG_PICTURES_DIR");

    /// <summary>
    /// <para>
    /// Defines the directory relative to which user-specific video files should be stored.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_VIDEOS_DIR</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default value is used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>Videos</c> known folder, usually pointing at <c>%USERPROFILE%\Videos</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Movies</c>
    /// </item>
    /// <item>
    /// On Unix: <c>~/Videos</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgUserDirectory Videos { get; } = new("XDG_VIDEOS_DIR");

    /// <summary>
    /// <para>
    /// Defines the directory relative to which user-specific template files should be stored.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_TEMPLATES_DIR</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default value is used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>Templates</c> known folder, usually pointing at <c>%APPDATA%\Microsoft\Windows\Templates</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Templates</c>
    /// </item>
    /// <item>
    /// On Unix: <c>~/Templates</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgUserDirectory Templates { get; } = new("XDG_TEMPLATES_DIR");

    /// <summary>
    /// <para>
    /// Defines the directory relative to which publicly shareable files should be stored.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_PUBLICSHARE_DIR</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default value is used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>Public</c> known folder, usually pointing at <c>%PUBLIC%</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Public</c>
    /// </item>
    /// <item>
    /// On Unix: <c>~/Public</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgUserDirectory Public { get; } = new("XDG_PUBLICSHARE_DIR");
}
