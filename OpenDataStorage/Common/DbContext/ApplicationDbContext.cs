using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenDataStorage.Common.DbContext.DbSetManagers;
using OpenDataStorage.Common.DbContext.NestedSets;
using OpenDataStorageCore.Entities;
using OpenDataStorageCore.Entities.CharacteristicValues;
using OpenDataStorageCore.Entities.NestedSets;

namespace OpenDataStorage.Common.DbContext
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        private INestedSetsObjectContext<HierarchyObject> _objectDbSetManager;
        private INestedSetsFSContext<Characteristic> _characteristicDbSetManager;
        private INestedSetsFSContext<ObjectType> _objectTypeDbSetManager;
        private ICharacteristicValueDbSetManager _characteristicValueDbSetManager;

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Database.Connection.ConnectionString = this.Database.Connection.ConnectionString + ";MultipleActiveResultSets=true;";

            _objectDbSetManager = new HierarchyObjectDbContextManager(HierarchyObjects, this.Database);
            _characteristicDbSetManager = new CharacteristicDbSetManager(Characteristics, this.Database);
            _objectTypeDbSetManager = new ObjectTypeDbSetManager(ObjectTypes, this.Database);
            _characteristicValueDbSetManager = new CharacteristicValueDbSetManager<BaseCharacteristicValue>(CharacteristicValues, this.SaveDbChangesAsync);
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
                m.ToTable(_characteristicDbSetManager.TableName);
            });

            modelBuilder.Entity<HierarchyObject>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable(_objectDbSetManager.TableName);
            });

            modelBuilder.Entity<ObjectType>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable(_objectTypeDbSetManager.TableName);
            });

            modelBuilder.Entity<BaseCharacteristicValue>().Map(m =>
            {
               m.MapInheritedProperties();
               m.ToTable(_characteristicValueDbSetManager.TableName);
            });

            modelBuilder.Entity<BaseCharacteristicValue>()
                .HasRequired(v => v.Characteristic)
                .WithMany()
                .HasForeignKey(v => v.CharacterisitcId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<BaseCharacteristicValue>()
                .HasRequired(c => c.HierarchyObject)
                .WithMany()
                .HasForeignKey(v => v.HierarchyObjectId)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<HierarchyObject> HierarchyObjects { get; set; }

        public DbSet<Characteristic> Characteristics { get; set; }

        public DbSet<ObjectType> ObjectTypes { get; set; }

        public DbSet<BaseCharacteristicValue> CharacteristicValues { get; set; }

        INestedSetsObjectContext<HierarchyObject> IApplicationDbContext.HierarchyObjectContext => this._objectDbSetManager;

        INestedSetsFSContext<Characteristic> IApplicationDbContext.CharacteristicContext => this._characteristicDbSetManager;

        INestedSetsFSContext<ObjectType> IApplicationDbContext.ObjectTypeContext => this._objectTypeDbSetManager;

        ICharacteristicValueDbSetManager IApplicationDbContext.CharacteristicValueDbSetManager => this._characteristicValueDbSetManager;

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

        public async Task ReloadFromDb(object entity)
        {
            await this.Entry(entity).ReloadAsync();
        }
    }
}
 