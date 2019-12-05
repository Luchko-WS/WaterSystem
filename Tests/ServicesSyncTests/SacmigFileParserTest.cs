using System.IO;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SyncOpenDateServices.SacmigFormat;

namespace ServicesSyncTests
{
    [TestClass]
    public class SacmigFileParserTest
    {
        [TestMethod]
        public void GetDataTest()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SacmigFileData data;
                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open))
                    {
                        var parser = new SacmigFileParser();
                        data = parser.Parse(stream);
                    }

                    var sz = JsonConvert.SerializeObject(data);
                    using (var sw = new StreamWriter($"{openFileDialog.SafeFileName}.json"))
                    {
                        sw.WriteLine(sz);
                    }
                }
            }
        }
    }
}
