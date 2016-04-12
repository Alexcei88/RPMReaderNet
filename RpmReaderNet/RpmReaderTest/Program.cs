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
            var list1 = new List<string>()
            {
                "1", "2", "3", "4", "5", "6", "7"
            };

            var list2 = new List<string>()
            {
                "1", "2", "3", "4", "5", "6", "7"
            };

            var list3 = list1.Take(5).Concat(list2.Take(3));

            using (RpmReader reader = new RpmReader("esbautoupdater.rpm"))
            {
                if (reader.Validate())
                {
                    Console.WriteLine("File Names: ");
                    if (reader.BaseFileNames != null)
                    {
                        foreach (var s in reader.BaseFileNames)
                        {
                            Console.WriteLine(s);
                        }
                    }
                }

                Console.WriteLine(reader.ToString());

                //reader.ExtractPackage(@"e:\Axelot\");

                Console.ReadKey();
            }
        }
    }
}