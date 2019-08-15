using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SyncOpenDateServices
{
    public interface IStreamParser<TOut>
    {
        Task<ICollection<TOut>> ParseAsync(Stream stream);
    }
}
