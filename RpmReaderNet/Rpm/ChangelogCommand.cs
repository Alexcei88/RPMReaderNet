using ManyConsole;
using RpmReaderNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpmNet
{
    /// <summary>
    /// the command for print log of changes
    /// </summary>
    class ChangelogCommand
        : ConsoleCommand
    {
        public ChangelogCommand()
        {
            IsCommand("changelog", "print log of changes in package");

            HasAdditionalArguments(1, " <input rpm package>");

        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                using (RpmReader reader = new RpmReader(remainingArguments.Last()))
                {
                    Console.WriteLine(reader.Changelog);
                }
                return 0;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
        }
    }
}
