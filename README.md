## RPMReaderNetLib
**RPMReaderNetLib** is a library for reading and extracting files from a rpm  file. It written entirely in C# for the .NET platform. The library written based on the [rpm format](https://refspecs.linuxbase.org/LSB_3.1.0/LSB-Core-generic/LSB-Core-generic/pkgformat.html) and [librpm API documentation](http://rpm.org/api/4.4.2.2/index.html). <br>Based on this library created a program **RpmNet** for executing queries to a rpm file.

## Usage

#### *RpmReaderNetLib* library

[Examples of the use of the library](https://github.com/Alexcei88/RPMReaderNet/wiki/Usage-RpmReaderNet)

#### *RpmNet* program

##### Install

Build the project and after the run the output file `RpmNetSetup.msi`.
The program *RpmNet* will be install to your computer to `c:\Program Files (x86)\RpmNet\` default.

##### A list all commands that are supported by the RpmNet program

- *info* - print a common information about package
- *filelist* - print a list all files in package
- *script* - print the contents of scripts
- *extract* - extract all files from package




## Nuget

Available on Nuget: https://www.nuget.org/packages/RpmReaderNet/

## License

RPMReaderNetLib are released under the GNU GPLv3 license. See LICENSE for details.

## Support

Find a bug? Contact us via email lex9.darovskoi@gmail.com.
