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
                    /*
                    Console.WriteLine("Source: ");
                    if (reader.Source != null)
                    {
                        foreach (var s in reader.Source)
                        {
                            Console.WriteLine(s);
                        }
                    }*/
                    /*
                    Console.WriteLine("PreinScript: {0}", reader.PreinScript);
                    Console.WriteLine("PostinScript: {0}", reader.PostinScript);
                    Console.WriteLine("PreunScript: {0}", reader.PreunScript);
                    Console.WriteLine("PostunScript: {0}", reader.PostunScript);
                    */
                    /*
                    Console.WriteLine("FileUserNames: ");
                    if (reader.FileUserNames != null)
                    {
                        foreach (var s in reader.FileUserNames)
                        {
                            Console.WriteLine(s);
                        }
                    }*/

                    Console.WriteLine("FileSize: {0}", reader.FileSizes);
                    /*Console.WriteLine("MD5 Files: ");
                    if (reader.MD5Files != null)
                    {
                        foreach (var s in reader.MD5Files)
                        {
                            Console.WriteLine(s);
                        }
                    }*/

                }

                Console.WriteLine(reader.ToString());

                //reader.ExtractPackage(@"e:\Axelot\");

                Console.ReadKey();
            }

        }
    }
}
