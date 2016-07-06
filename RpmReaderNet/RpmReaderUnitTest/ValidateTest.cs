using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpmReaderNet;
using System.IO;
using System.Diagnostics;

namespace RpmReaderUnitTest
{
    [TestClass]
    public class ValidateTest
    {
        private TestContext testContextInstance;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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
                Assert.AreEqual(reader.Description, "VLC media player is a highly portable multimedia player and multimedia framework\ncapable of reading most audio and video formats as well as DVDs, Audio CDs VCDs,\nand various streaming protocols.\nIt can also be used as a media converter or a server to stream in uni-cast or\nmulti-cast in IPv4 or IPv6 on networks.");
                uint expectedSize = 34215049;
                Assert.AreEqual(reader.Size, expectedSize);
                Assert.AreEqual(reader.BuildTime, new DateTime(2016, 06, 26, 20, 15, 29));

                Assert.IsFalse(reader.Changelog == null);

                
         
            }
        }
    }
}
