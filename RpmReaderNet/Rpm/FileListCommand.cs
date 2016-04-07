using ManyConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpm
{
    /// <summary>
    /// Command to output list of files
    /// </summary>
    class FileListCommand
        : ConsoleCommand
    {
        public FileListCommand()
        {
            IsCommand("filelist", "print all files in package");

            HasAdditionalArguments(1, " <input rpm package>");
        }

        public override int Run(string[] remainingArguments)
        {
            throw new NotImplementedException();
        }
    }
}
