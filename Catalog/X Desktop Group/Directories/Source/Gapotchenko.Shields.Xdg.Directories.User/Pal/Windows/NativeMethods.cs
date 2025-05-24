namespace Gapotchenko.Shields.Xdg.Directories.User.Pal.Windows;

#if NET
[SupportedOSPlatform("windows")]
#endif
static class NativeMethods
{
    [DllImport("shell32", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
    public static extern string SHGetKnownFolderPath(in Guid rfid, int dwFlags, IntPtr hToken);
}
