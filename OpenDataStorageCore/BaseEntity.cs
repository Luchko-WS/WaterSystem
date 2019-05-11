using System;

namespace OpenDataStorageCore
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.Now;
        }

        public Guid Id { get; set; }

        public string OwnerId { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}
