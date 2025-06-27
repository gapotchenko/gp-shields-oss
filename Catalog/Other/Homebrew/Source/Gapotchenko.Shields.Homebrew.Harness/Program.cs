using Gapotchenko.FX.Linq;
using Gapotchenko.FX.Math.Intervals;
using Gapotchenko.Shields.Homebrew.Deployment;

namespace Gapotchenko.Shields.Homebrew.Harness;

class Program
{
    static void Main()
    {
        try
        {
            ListSetupInstances();
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
            BrewDeployment.EnumerateSetupInstances(ValueInterval.Infinite<Version>())
            .Zip(Enumerable.Range(1, int.MaxValue)))
        {
            Console.WriteLine("#{0}", i);
            PrintSetupInstance(instance);

            Console.WriteLine();
        }
    }

    static void PrintSetupInstance(IBrewSetupInstance instance, int level = 0)
    {
        string padding = new(' ', level * 4);

        Console.Write(padding);
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Product ID: {0}", "Homebrew.Product");
        Console.ResetColor();
        Console.WriteLine();

        Console.Write(padding);
        Console.WriteLine("Display name: {0}", instance.DisplayName);

        Console.Write(padding);
        Console.WriteLine("Installation path: {0}", instance.InstallationPath);

        Console.Write(padding);
        Console.WriteLine("Product path: {0}", instance.ResolvePath(instance.ProductPath));

        Console.Write(padding);
        Console.WriteLine("Cellar path: {0}", instance.ResolvePath("Cellar"));
    }
}
