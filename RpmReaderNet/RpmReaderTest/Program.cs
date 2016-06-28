using RpmReaderNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RpmReaderTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (RpmReader reader = new RpmReader("esbautoupdater.rpm"))
            {
                
                if (reader.Validate())
                {
                    /*
                    Console.WriteLine("File Names: ");
                    if (reader.BaseFileNames != null)
                    {
                        foreach (var s in reader.BaseFileNames)
                        {
                            Console.WriteLine(s);
                        }
                    }*/
                    Console.WriteLine(reader.ToString());

                    //reader.ExtractPackage(@"e:\Axelot\");


                }

                Console.ReadKey();
            }
        }
    }
}