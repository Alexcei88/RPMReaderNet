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
                    Console.WriteLine("Arch: {0}", reader.Arch);
                    Console.WriteLine("PreinScript: {0}", reader.PreinScript);
                    Console.WriteLine("PostinScript: {0}", reader.PostinScript);
                    Console.WriteLine("PreunScript: {0}", reader.PreunScript);
                    Console.WriteLine("PostunScript: {0}", reader.PostunScript);
                    Console.WriteLine("FileNames: ");
                    if (reader.FileUserNames != null)
                    {
                        foreach (var s in reader.FileUserNames)
                        {
                            Console.WriteLine(s);
                        }
                    }

                    Console.WriteLine("FileSize: {0}", reader.FileSizes);
                    Console.WriteLine("MD5 Files: ");
                    if (reader.MD5Files != null)
                    {
                        foreach (var s in reader.MD5Files)
                        {
                            Console.WriteLine(s);
                        }
                    }

                }

                Console.WriteLine(reader.ToString());
                //reader.SaveArchive(AppDomain.CurrentDomain.BaseDirectory);

                Console.ReadKey();
            }

        }
    }
}
