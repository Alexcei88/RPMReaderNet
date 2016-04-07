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
        private bool _printAllScripts;

        public OutputScriptCommand()
        {
            IsCommand("script", "prints the contents of scripts");

            HasOption("all", "print all scripts", b => _printAllScripts = true);

            HasAdditionalArguments(1, "input rpm package");
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                using (RpmReader reader = new RpmReader(remainingArguments.Last()))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append($"Name: {reader.Name}\n");
                    builder.Append($"Version: {reader.Version}\n");
                    builder.Append($"Release: {reader.Release}\n");
                    builder.Append($"Architecture: {reader.Arch}\n");
                    builder.Append($"Size: {reader.Size}\n");
                    builder.Append($"License: {reader.License}\n");
                    builder.Append($"Source RPM: {reader.SourceRpm}\n");
                    builder.Append($"BuildTime: {reader.BuildTime}\n");
                    builder.Append($"BuildHost: {reader.BuildHost}\n");
                    builder.Append($"Summary: {reader.Summary}\n");
                    builder.Append($"Description: {reader.Description}\n");
                    Console.WriteLine(builder.ToString());
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

        private void PrintPreinScript(StringBuilder builder, RpmReader reader)
        {
            builder.Append("The Prein scipts content:\n");
            builder.Append(reader.PreinScript + "\n");
            PrintSeparator(builder);
        }

        private void PrintPostinScript(StringBuilder builder, RpmReader reader)
        {
            builder.Append("The Postin scipts content:\n");
            builder.Append(reader.PostinScript + "\n");
            PrintSeparator(builder);
        }

        private void PrintPreunScript(StringBuilder builder, RpmReader reader)
        {
            builder.Append("The Preun scipts content:\n");
            builder.Append(reader.PreunScript + "\n");
            PrintSeparator(builder);
        }

        private void PrintPostunScript(StringBuilder builder, RpmReader reader)
        {
            builder.Append("The Postun scipts content:\n");
            builder.Append(reader.PostunScript + "\n");
            PrintSeparator(builder);
        }

        private void PrintSeparator(StringBuilder builder)
        {
            builder.Append("*************************\n");
        }
    }
}
