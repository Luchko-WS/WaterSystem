using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyncOpenDateServices.SacmigFormat;

namespace ServicesSyncTests
{
    [TestClass]
    public class SacmigFileParserTest
    {
        [TestMethod]
        public async Task GetDataTest()
        {
            var streamReader = File.Open("test.xlsx", FileMode.Open);
            var parser = new SacmigFileParser();
            var data = await parser.ParseAsync(streamReader);
        }
    }
}
