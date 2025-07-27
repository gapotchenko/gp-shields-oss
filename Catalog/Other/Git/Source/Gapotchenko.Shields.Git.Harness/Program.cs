// Gapotchenko.Shields.Git
//
// Copyright © Gapotchenko and Contributors
//
// File introduced by: Oleksiy Gapotchenko
// Year of introduction: 2025

using Gapotchenko.FX.Linq;
using Gapotchenko.FX.Math.Intervals;
using Gapotchenko.Shields.Git.Deployment;

namespace Gapotchenko.Shields.Git.Harness;

class Program
{
    static void Main()
    {
        try
        {
            ListSetupInstances();
            ListPortableSetupInstances();
        }
        catch (Exception e)
        {
            Console.Write("Error: ");
            Console.WriteLine(e);
        }
    }

    static void ListSetupInstances()
    {
        Console.WriteLine("*** Installed Setup Instances ***");
        Console.WriteLine();

        foreach (var (instance, i) in
            GitDeployment.EnumerateSetupInstances(ValueInterval.Infinite<Version>())
            .Zip(Enumerable.Range(1, int.MaxValue)))
        {
            Console.WriteLine("#{0}", i);
            PrintSetupInstance(instance);

            Console.WriteLine();
        }
    }

    static void ListPortableSetupInstances()
    {
        Console.WriteLine("*** Portable Setup Instances ***");
        Console.WriteLine();

        string[] paths = [@"C:\Program Files\Git"];

        foreach (var (path, i) in paths.Zip(Enumerable.Range(1, int.MaxValue)))
        {
            Console.WriteLine("#{0} at '{1}'", i, path);

            var instance = GitSetupInstance.TryOpen(path);
            if (instance is null)
                Console.WriteLine("Git setup instance is not found.");
            else
                PrintSetupInstance(instance);

            Console.WriteLine();
        }
    }

    static void PrintSetupInstance(IGitSetupInstance instance, int level = 0)
    {
        string padding = new(' ', level * 4);

        Console.Write(padding);
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Product ID: {0}", "Git.Product");
        Console.ResetColor();
        Console.WriteLine();

        Console.Write(padding);
        Console.WriteLine("Display name: {0}", instance.DisplayName);

        Console.Write(padding);
        Console.WriteLine("Installation path: {0}", instance.InstallationPath);

        Console.Write(padding);
        Console.WriteLine("Product path: {0}", instance.ResolvePath(instance.ProductPath));
    }
}
