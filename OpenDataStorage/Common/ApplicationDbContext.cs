using System.Data.Entity;
using OpenDataStorage.Core.DataAccessLayer.DbContext;

namespace OpenDataStorage.Common
{
    public sealed class ApplicationDbContext : OpenDataStorageDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
 