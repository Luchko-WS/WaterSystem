using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyncOpenDateServices.TextyOrgUaWater;

namespace ServicesSyncTests
{
    [TestClass]
    public class TextyOrgUaWaterServiceTest
    {
        [TestMethod]
        public async Task GetReport()
        {
            var service = new TextyOrgUaWaterService();
            var data = await service.GetData();
        }
    }
}
