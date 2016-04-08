using ManyConsole;
using NDesk.Options;
using RpmReaderNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpm
{
    /// <summary>
    /// extract files from package command
    /// </summary>
    class ExtractCommand
        : ConsoleCommand
    {
        /// <summary>
        /// Destinition folder
        /// </summary>
        private string _destFolder;

        public ExtractCommand()
        {
            IsCommand("extract", "extract all files from package");
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
