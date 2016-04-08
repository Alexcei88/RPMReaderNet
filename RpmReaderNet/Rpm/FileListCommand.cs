﻿using ManyConsole;
using RpmReaderNet;
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
            IsCommand("filelist", "prints a list of all files in package");

            HasAdditionalArguments(1, " <input rpm package>");
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {

                using (RpmReader reader = new RpmReader(remainingArguments.Last()))
                {
                    string[] baseNames = reader.BaseFileNames;
                    string[] dirs = reader.DirNames;
                    uint[] dirIndexes = reader.DirIndexes;


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
