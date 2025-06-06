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
            Run1();
        }
        catch (Exception e)
        {
            Console.Write("Error: ");
            Console.WriteLine(e);
        }
    }

    static void Run1()
    {
        Console.WriteLine("*** Cygwin Setup Instances ***");
        Console.WriteLine();

        foreach (var (i, instance) in
            Enumerable.Range(1, int.MaxValue)
            .Zip(CygwinDeployment.EnumerateSetupInstances(ValueInterval.Infinite<Version>())))
        {
            Console.WriteLine("#{0}", i);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Product ID: {0}", "Cygwin.Product");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Display name: {0}", instance.DisplayName);
            Console.WriteLine("Installation path: {0}", instance.InstallationPath);
            Console.WriteLine("Product path: {0}", instance.ResolvePath(instance.ProductPath));

            Console.WriteLine();
        }
    }
}
