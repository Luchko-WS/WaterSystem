using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenDataStorageCore;

namespace OpenDataStorage.Common
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { 
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<HierarchyObject> HierarchyObjects { get; set; }

        public DbSet<Characteristic> Characteristics { get; set; }

        IQueryable<HierarchyObject> IApplicationDbContext.HierarchyObjects => this.HierarchyObjects;

        IQueryable<Characteristic> IApplicationDbContext.Characteristics => this.Characteristics;

        public async Task SaveDbChangesAsync()
        {
            using (var transaction = this.Database.BeginTransaction())
            {
                try
                {
                    await this.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}