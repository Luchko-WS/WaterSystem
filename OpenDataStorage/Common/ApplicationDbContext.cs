using OpenDataStorage.Core.DataAccessLayer.DbContext;

namespace OpenDataStorage.Common
{
    public sealed class ApplicationDbContext : OpenDataStorageDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false) { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
 