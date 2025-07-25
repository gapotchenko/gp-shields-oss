﻿using Gapotchenko.FX.Linq;
using Gapotchenko.FX.Math.Intervals;
using Gapotchenko.Shields.Microsoft.Wsl.Deployment;
using Gapotchenko.Shields.Microsoft.Wsl.Runtime;

namespace Gapotchenko.Shields.Microsoft.Wsl.Harness;

class Program
{
    static void Main()
    {
        try
        {
            ListSetupInstances();
            ListRunningInstances();
        }
        catch (Exception e)
        {
            Console.Write("Error: ");
            Console.WriteLine(e);
        }
    }

    static void ListSetupInstances()
    {
        Console.WriteLine("*** Setup Instances ***");
        Console.WriteLine();

        foreach (var (instance, i) in
            WslDeployment.EnumerateSetupInstances(ValueInterval.Infinite<Version>())
            .Zip(Enumerable.Range(1, int.MaxValue)))
        {
            Console.WriteLine("#{0}", i);
            PrintSetupInstance(instance);

            Console.WriteLine();
        }
    }

    static void ListRunningInstances()
    {
        Console.WriteLine("*** Running Instance ***");
        Console.WriteLine();

        var instance = WslRuntime.RunningInstance;
        if (instance != null)
            PrintRunningInstance(instance);
        else
            Console.WriteLine("Not running under WSL.");
    }

    static void PrintRunningInstance(IWslRunningInstance instance, int level = 0)
    {
        string padding = new(' ', level * 4);

        Console.Write(padding);
        Console.WriteLine("Distribution name: {0}", instance.DistributionName);

        Console.Write(padding);
        Console.WriteLine("Setup:");
        PrintSetupInstance(instance.Setup, level + 1);
    }

    static void PrintSetupInstance(IWslSetupInstance instance, int level = 0)
    {
        string padding = new(' ', level * 4);

        Console.Write(padding);
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Product ID: {0}", "Microsoft.WSL.Product");
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
