using System;

namespace OpenDataStorageCore
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string OwnerId { get; set; }
    }
}
