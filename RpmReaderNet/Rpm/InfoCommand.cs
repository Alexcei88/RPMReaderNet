using ManyConsole;
using RpmReaderNet;
using System;
using System.Text;

namespace Rpm
{
    /// <summary>
    /// Command for getting info package
    /// </summary>
    internal class InfoCommand
        : ConsoleCommand
    {
        /// <summary>
        /// path to package
        /// </summary>
        private string _path;

        public InfoCommand()
        {
            IsCommand("info", "Outputs info package");
            HasAdditionalArguments(1, " <input rpm package>");
        }

        public override int Run(string[] remainingArguments)
        {
            _path = remainingArguments[0];

            try
            {
                using (RpmReader reader = new RpmReader(_path))
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
    }
}