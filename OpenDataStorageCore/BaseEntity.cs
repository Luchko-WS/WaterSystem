using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDataStorageCore
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = new Guid();
        }

        public Guid Id { get; set; }
    }
}
