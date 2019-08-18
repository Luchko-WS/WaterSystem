using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyncOpenDateServices.SacmigFormat;

namespace ServicesSyncTests
{
    [TestClass]
    public class SacmigFileParserTest
    {
        [TestMethod]
        public void GetDataTest()
        {
            var stream = File.Open("test.xlsx", FileMode.Open);
            var parser = new SacmigFileParser();
            var data = parser.Parse(stream);
            stream.Close();
        }
    }
}
