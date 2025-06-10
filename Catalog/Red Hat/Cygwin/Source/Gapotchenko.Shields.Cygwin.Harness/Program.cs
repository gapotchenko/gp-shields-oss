using Gapotchenko.FX.Linq;
using Gapotchenko.FX.Math.Intervals;
using Gapotchenko.Shields.Cygwin.Deployment;

namespace Gapotchenko.Shields.Cygwin.Harness;

class Program
{
    static void Main()
    {
        try
        {
            ListInstalledSetupInstances();
            ListPortableSetupInstances();
        }
        catch (Exception e)
        {
            Console.Write("Error: ");
            Console.WriteLine(e);
        }
    }

    static void ListInstalledSetupInstances()
    {
        Console.WriteLine("*** Installed Cygwin Setup Instances ***");
        Console.WriteLine();

        foreach (var (instance, i) in
            CygwinDeployment.EnumerateSetupInstances(ValueInterval.Infinite<Version>())
            .Zip(Enumerable.Range(1, int.MaxValue)))
        {
            Console.WriteLine("#{0}", i);
            PrintSetupInstance(instance);

            Console.WriteLine();
        }
    }

    static void ListPortableSetupInstances()
    {
        Console.WriteLine("*** Portable Cygwin Setup Instances ***");
        Console.WriteLine();

        string[] paths = [@"C:\cygwin64"];

        foreach (var (path, i) in paths.Zip(Enumerable.Range(1, int.MaxValue)))
        {
            Console.WriteLine("#{0} at '{1}'", i, path);

            var instance = CygwinSetupInstance.TryOpen(path);
            if (instance is null)
                Console.WriteLine("Cygwin setup instance is not found.");
            else
                PrintSetupInstance(instance);

            Console.WriteLine();
        }
    }

    static void PrintSetupInstance(ICygwinSetupInstance instance)
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Product ID: {0}", "Cygwin.Product");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("Display name: {0}", instance.DisplayName);
        Console.WriteLine("Installation path: {0}", instance.InstallationPath);
        Console.WriteLine("Product path: {0}", instance.ResolvePath(instance.ProductPath));
    }
}
