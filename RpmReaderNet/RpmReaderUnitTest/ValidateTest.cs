using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpmReaderNet;
using System.IO;

namespace RpmReaderUnitTest
{
    [TestClass]
    public class ValidateTest
    {
        /// <summary>
        /// Execute a validation of input file and check metadata from package
        /// </summary>
        [TestMethod]
        public void ValidateFile()
        {
            using (RpmReader reader = new RpmReader("vlc.rpm"))
            {
                // проверяем общие свойства
                Assert.IsTrue(reader.IsValidate);
                Assert.AreEqual(reader.Name, "vlc");
                Assert.AreEqual(reader.Version, "3.0.0");
                Assert.AreEqual(reader.Release, "7.20160608gitbb83680.fc24");
                Assert.AreEqual(reader.Arch, "x86_64");
                Assert.AreEqual(reader.License, "GPLv2+");
                Assert.AreEqual(reader.BuildHost, "copr-builder-856817140.novalocal");
                Assert.AreEqual(reader.Summary, "The cross-platform open-source multimedia framework, player and server");
                Assert.AreEqual(reader.Vendor, "Fedora Project COPR (paulcarroty/test-3.0.2)");
                Assert.AreEqual(reader.SourceRpm, null);

                //Assert.AreEqual(reader.Description, "VLC media player is a highly portable multimedia player and multimedia framework capable of reading most audio and video formats as well as DVDs, Audio CDs VCDs, and various streaming protocols. It can also be used as a media converter or a server to stream in uni - cast or multi - cast in IPv4 or IPv6 on networks.");
                //Assert.AreEqual(reader.Size, 34215049);
                //Assert.AreEqual(reader.BuildTime, new DateTime(2016, 06, 26, ))


                Assert.AreEqual(reader.Changelog, File.ReadAllText("changelog_standart.txt"));

                
         
            }
        }
    }
}
