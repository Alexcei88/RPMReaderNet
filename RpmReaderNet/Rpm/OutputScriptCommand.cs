using ManyConsole;
using RpmReaderNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpm
{
    /// <summary>
    /// Command for output scripts
    /// </summary>
    class OutputScriptCommand
        : ConsoleCommand
    {
        /// <summary>
        /// Mark to prints all script
        /// </summary>
        private bool _printAllScripts;

        /// <summary>
        /// Mark to prints preinscript
        /// </summary>
        private bool _printPreinScript;

        /// <summary>
        /// Mark to prints postinscript
        /// </summary>
        private bool _printPostinScript;

        /// <summary>
        /// Mark to prints preunscript
        /// </summary>
        private bool _printPreunScript;

        /// <summary>
        /// Mark to prints postunscript
        /// </summary>
        private bool _printPostunScript;


        public OutputScriptCommand()
        {
            IsCommand("script", "prints the contents of scripts");

            HasOption("all", "prints all scripts", b => _printAllScripts = true);
            HasOption("prein", "prints the preinstall script", b => _printPreinScript = true);
            HasOption("postin", "prints the postinstall script", b => _printPostinScript = true);
            HasOption("preun", "prints the preuninstall script", b => _printPreunScript = true);
            HasOption("postun", "prints the postuninstall script", b => _printPostunScript = true);

            HasAdditionalArguments(1, "input rpm package");
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                using (RpmReader reader = new RpmReader(remainingArguments.Last()))
                {
                    StringBuilder builder = new StringBuilder();
                    if (_printAllScripts || (!_printPreinScript && !_printPostinScript && !_printPreunScript && !_printPostunScript))
                    {
                        PrintPreinScript(builder, reader);
                        PrintPostinScript(builder, reader);
                        PrintPreunScript(builder, reader);
                        PrintPostunScript(builder, reader);
                        Console.WriteLine(builder.ToString());
                        return 0;
                    }
                    if(_printPreinScript)
                    {
                        PrintPreinScript(builder, reader);
                    }
                    if(_printPostinScript)
                    {
                        PrintPostinScript(builder, reader);
                    }
                    if(_printPreunScript)
                    {
                        PrintPreunScript(builder, reader);
                    }
                    if(_printPostunScript)
                    {
                        PrintPostunScript(builder, reader);
                    }
                    Console.WriteLine(builder.ToString());
                    return 0;
                }
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

        private void PrintPreinScript(StringBuilder builder, RpmReader reader)
        {
            builder.Append("The Preinstall scripts content:\n");
            builder.Append(reader.PreinScript + "\n");
            PrintSeparator(builder);
        }

        private void PrintPostinScript(StringBuilder builder, RpmReader reader)
        {
            builder.Append("The Postinstall scripts content:\n");
            builder.Append(reader.PostinScript + "\n");
            PrintSeparator(builder);
        }

        private void PrintPreunScript(StringBuilder builder, RpmReader reader)
        {
            builder.Append("The Preuninstall scripts content:\n");
            builder.Append(reader.PreunScript + "\n");
            PrintSeparator(builder);
        }

        private void PrintPostunScript(StringBuilder builder, RpmReader reader)
        {
            builder.Append("The Postuninstall scripts content:\n");
            builder.Append(reader.PostunScript + "\n");
            PrintSeparator(builder);
        }

        private void PrintSeparator(StringBuilder builder)
        {
            builder.Append("*************************\n");
        }
    }
}
