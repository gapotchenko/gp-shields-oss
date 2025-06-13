namespace Gapotchenko.Shields.MSys2.Deployment.Utils;

static class VersionUtil
{
    public static void Deconstruct(this Version version, out int major, out int minor, out int build)
    {
        major = version.Major;
        minor = version.Minor;
        build = version.Build;
    }
}
