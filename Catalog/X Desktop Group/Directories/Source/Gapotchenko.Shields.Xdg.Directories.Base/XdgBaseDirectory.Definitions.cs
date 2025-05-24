// Gapotchenko.Shields.Xdg.Directories.Base
// Copyright © Gapotchenko
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2023

namespace Gapotchenko.Shields.Xdg.Directories.Base;

partial struct XdgBaseDirectory
{
    /// <summary>
    /// <para>
    /// Defines the base directory relative to which user-specific data files should be stored.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_DATA_HOME</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default value is used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>%LOCALAPPDATA%</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Library/Application Support</c>
    /// </item>
    /// <item>
    /// On Unix: <c>~/.local/share</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgBaseDirectory DataHome { get; } = new("XDG_DATA_HOME");

    /// <summary>
    /// <para>
    /// Defines the preference-ordered set of base directories to search for data files in addition to the <see cref="DataHome"/> base directory.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_DATA_DIRS</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default values are used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>%APPDATA%;%ProgramData%</c>
    /// </item>
    /// <item>
    /// On macOS: <c>/Library/Application Support</c>
    /// </item>
    /// <item>
    /// On Unix: <c>/usr/local/share:/usr/share</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgBaseDirectory DataDirectories { get; } = new("XDG_DATA_DIRS");

    /// <summary>
    /// <para>
    /// Defines the base directory relative to which user-specific configuration files should be stored.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_CONFIG_HOME</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default value is used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>%LOCALAPPDATA%</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Library/Application Support</c>
    /// </item>
    /// <item>
    /// On Unix: <c>~/.config</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgBaseDirectory ConfigurationHome { get; } = new("XDG_CONFIG_HOME");

    /// <summary>
    /// <para>
    /// Defines the preference-ordered set of base directories to search for configuration files in addition to the <see cref="ConfigurationHome"/> base directory.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_CONFIG_DIRS</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default values are used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>%ProgramData%;%APPDATA%</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Library/Preferences:/Library/Application Support:/Library/Preferences</c>
    /// </item>
    /// <item>
    /// On Unix: <c>/etc/xdg</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgBaseDirectory ConfigurationDirectories { get; } = new("XDG_CONFIG_DIRS");

    /// <summary>
    /// <para>
    /// Defines the base directory relative to which user-specific state files should be stored.
    /// The state contains data that should persist between (application) restarts,
    /// but that is not important or portable enough to the user that it should be stored in <see cref="DataHome"/>.
    /// It may contain:
    /// <list type="bullet">
    /// <item>
    /// actions history (logs, history, recently used files, …)
    /// </item>
    /// <item>
    /// current state of the application that can be reused on a restart (view, layout, open files, undo history, …)
    /// </item>
    /// </list>
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_STATE_HOME</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default value is used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>%LOCALAPPDATA%</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Library/Application Support</c>
    /// </item>
    /// <item>
    /// On Unix: <c>~/.local/state</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgBaseDirectory StateHome { get; } = new("XDG_STATE_HOME");

    /// <summary>
    /// <para>
    /// Defines the base directory relative to which user-specific non-essential data files should be stored.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_CACHE_HOME</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the environment variable is either not set or empty, the following default value is used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>%LOCALAPPDATA%\cache</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Library/Caches</c>
    /// </item>
    /// <item>
    /// On Unix: <c>~/.cache</c>
    /// </item>
    /// </list>
    /// </remarks>
    public static XdgBaseDirectory CacheHome { get; } = new("XDG_CACHE_HOME");

    /// <summary>
    /// <para>
    /// Defines the base directory relative to which user-specific non-essential runtime files and other file objects (such as sockets, named pipes, ...) should be stored.
    /// </para>
    /// <para>
    /// Corresponds to <c>XDG_RUNTIME_DIR</c> environment variable.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// The directory MUST be on a local file system and not shared with any other system.
    /// The directory MUST by fully-featured by the standards of the operating system.
    /// More specifically, on Unix-like operating systems <c>AF_UNIX</c> sockets, symbolic links, hard links, proper permissions, file locking, sparse files, memory mapping, file change notifications, a reliable hard link count must be supported, and no restrictions on the file name character set should be imposed.
    /// Files in this directory MAY be subjected to periodic clean-up.
    /// To ensure that your files are not removed, they should have their access time timestamp modified at least once every 6 hours of monotonic time or the 'sticky' bit should be set on the file.
    /// </para>
    /// <para>
    /// The directory MUST be owned by the user, and he MUST be the only one having read and write access to it.
    /// Its Unix access mode MUST be <c>0700</c>.
    /// </para>
    /// <para>
    /// If the environment variable is either not set or empty, the following default value is used:
    /// <list type="bullet">
    /// <item>
    /// On Windows: <c>%LOCALAPPDATA%</c>
    /// </item>
    /// <item>
    /// On macOS: <c>~/Library/Application Support</c>
    /// </item>
    /// <item>
    /// On Unix: <c>/run/user/UID</c>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    public static XdgBaseDirectory RuntimeDirectory { get; } = new("XDG_RUNTIME_DIR");
}
