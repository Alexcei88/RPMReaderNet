using ManyConsole;
using Mono.Options;
using RpmReaderNet;
using System;
using System.Linq;

namespace Rpm
{
    /// <summary>
    /// command to extract files from the package 
    /// </summary>
    internal class ExtractCommand
        : ConsoleCommand
    {
        /// <summary>
        /// Destinition folder
        /// </summary>
        private string _destFolder;

        public ExtractCommand()
        {
            IsCommand("extract", "extract all files from the package");
            Options = new OptionSet()
            {
                { "d|destinition", "Destinition folder", d => _destFolder = d }
            };

            HasAdditionalArguments(2, "<input rpm package>");
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                using (RpmReader reader = new RpmReader(remainingArguments.Last()))
                {
                    reader.ExtractPackage(remainingArguments[0]);
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