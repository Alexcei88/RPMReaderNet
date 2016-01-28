using RpmReaderNet;
using System;
using System.Collections.Generic;
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
                    System.Console.WriteLine(reader.Version);
                }
            }

        }
    }
}
