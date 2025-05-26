// Gapotchenko.Shields.Java
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2019

namespace Gapotchenko.Shields.Java.Deployment;

sealed class JavaSetupInstanceEnv(string homePath) :
    JavaSetupInstanceFS(homePath, null)
{
    public static JavaSetupInstanceEnv? TryCreate()
    {
        string? javaHome = Environment.GetEnvironmentVariable("JAVA_HOME");
        if (!Directory.Exists(javaHome))
            return null;

        return new JavaSetupInstanceEnv(javaHome);
    }
}
