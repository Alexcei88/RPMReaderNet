using RpmReaderNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpmReaderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (RpmReader reader = new RpmReader("esbadapterhost.rpm"))
            {
                if (reader.Validate())
                {
                    Console.WriteLine("Name: {0}", reader.Name);
                    Console.WriteLine("Version: {0}", reader.Version);
                    Console.WriteLine("Release: {0}", reader.Release);
                    Console.WriteLine("Serial: {0}", reader.Serial);
                    Console.WriteLine("Summary: {0}", reader.Summary);
                    Console.WriteLine("Description: {0}", reader.Description);
                    Console.WriteLine("BuildTime: {0}", reader.BuildTime);
                    Console.WriteLine("BuildHost: {0}", reader.BuildHost);
                    Console.WriteLine("Distribution: {0}", reader.Distribution);
                    Console.WriteLine("Vendor: {0}", reader.Vendor);
                    Console.WriteLine("License: {0}", reader.License);
                    Console.WriteLine("Packager: {0}", reader.Packager);
                    Console.WriteLine("Changelog: {0}", reader.Changelog);
                    Console.WriteLine("Source: ");
                    if (reader.Source != null)
                    {
                        foreach (var s in reader.Source)
                        {
                            Console.WriteLine(s);
                        }
                    }
                }

                //reader.SaveArchive(AppDomain.CurrentDomain.BaseDirectory);

                Console.ReadKey();
            }

        }
    }
}
