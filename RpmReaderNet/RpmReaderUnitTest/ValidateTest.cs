using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpmReaderNet;

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
            using (RpmReader reader = new RpmReader("esbautoupdater.rpm"))
            {
                Assert.IsTrue(reader.IsValidate);
         
            }
        }
    }
}
