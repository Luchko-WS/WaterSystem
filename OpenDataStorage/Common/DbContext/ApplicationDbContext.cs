using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenDataStorageCore;

namespace OpenDataStorage.Common.DbContext
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        private HierarchyObjectDbContextManager _objectDbContextManager;
        private CharacteristicDbContextManager _characteristicDbContextManager;

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            _objectDbContextManager = new HierarchyObjectDbContextManager(HierarchyObjects, this.Database);
            _characteristicDbContextManager = new CharacteristicDbContextManager(Characteristics, this.Database);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Characteristic>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable(_characteristicDbContextManager.TableName);
            });

            modelBuilder.Entity<HierarchyObject>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable(_objectDbContextManager.TableName);
            });

            modelBuilder.Entity<CharacteristicValue>().Map(m =>
            {
               m.MapInheritedProperties();
               m.ToTable("Values");
            });

            modelBuilder.Entity<CharacteristicValue>()
                .HasRequired(v => v.Characteristic)
                .WithMany();

            modelBuilder.Entity<CharacteristicValue>()
                .HasRequired(c => c.HierarchyObject)
                .WithMany();

            base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<HierarchyObject> HierarchyObjects { get; set; }

        public DbSet<Characteristic> Characteristics { get; set; }

        public DbSet<CharacteristicValue> CharacteristicValues { get; set; }

        INestedSetsObjectContext<HierarchyObject> IApplicationDbContext.HierarchyObjectContext => this._objectDbContextManager;

        INestedSetsObjectContext<Characteristic> IApplicationDbContext.CharacteristicObjectContext => this._characteristicDbContextManager;

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
                    throw ex;
                }
            }
        }
    }
}
 