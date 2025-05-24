using Gapotchenko.FX.Math.Intervals;
using Gapotchenko.Shields.Java.Deployment;

namespace Gapotchenko.Shields.Java.Harness;

class Program
{
    static void Main()
    {
        foreach (var i in JavaDeployment.EnumerateSetupInstances(ValueInterval.Infinite<Version>()))
        {
            Console.WriteLine("*** Setup Instance ***");
            Console.WriteLine("Product ID: {0}", i.Product.Id);
            Console.WriteLine("Home path: {0}", i.HomePath);
            Console.WriteLine("Version: {0}", i.Version);
            Console.WriteLine();
        }
    }
}
