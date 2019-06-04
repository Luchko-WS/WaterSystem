using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SyncOpenDateServices.TextyOrgUaWater
{
    public interface ITextyOrgUaWaterServiceParser
    {
        Task<ICollection<TextyOrgUaWaterRow>> ParseAsync(Stream stream);
    }
}
