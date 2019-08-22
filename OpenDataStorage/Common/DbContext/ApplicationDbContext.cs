using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenDataStorage.Common.DbContext.DbSetManagers;
using OpenDataStorage.Common.DbContext.DbSetManagers.Aliases;
using OpenDataStorage.Common.DbContext.NestedSets;
using OpenDataStorageCore.Entities;
using OpenDataStorageCore.Entities.Aliases;
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
        private ICharacteristicAliasDbSetManager _characteristicAliasDbSetManager;
        private IHierarchyObjectAliasDbSetManager _hierarchyObjectAliasDbSetManager;

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());
            Configuration.ProxyCreationEnabled = false;

            _objectDbSetManager = new HierarchyObjectDbContextManager(HierarchyObjects, this.Database);
            _characteristicDbSetManager = new CharacteristicDbSetManager(Characteristics, this.Database);
            _objectTypeDbSetManager = new ObjectTypeDbSetManager(ObjectTypes, this.Database);
            _characteristicValueDbSetManager = new CharacteristicValueDbSetManager<BaseCharacteristicValue>(CharacteristicValues, this.SaveDbChangesAsync);
            _characteristicAliasDbSetManager = new CharacteristicAliasDbSetManager(CharacteristicAliases, this.SaveDbChangesAsync);
            _hierarchyObjectAliasDbSetManager = new HierarchyObjectAliasDbSetManager(HierarchyObjectAliases, this.SaveDbChangesAsync);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public async Task SaveDbChangesAsync()
        {
            await this.SaveChangesAsync();
        }

        public async Task ReloadFromDb(object entity)
        {
            await this.Entry(entity).ReloadAsync();
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

            modelBuilder.Entity<CharacteristicAlias>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable(_characteristicAliasDbSetManager.TableName);
            });

            modelBuilder.Entity<CharacteristicAlias>()
                .HasRequired(v => v.Characteristic)
                .WithMany(c => c.CharacteristicAliases)
                .HasForeignKey(v => v.CharacteristicId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<HierarchyObjectAlias>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable(_hierarchyObjectAliasDbSetManager.TableName);
            });

            modelBuilder.Entity<HierarchyObjectAlias>()
                .HasRequired(c => c.HierarchyObject)
                .WithMany(o => o.HierarchyObjectAliases)
                .HasForeignKey(v => v.HierarchyObjectId)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<HierarchyObject> HierarchyObjects { get; set; }

        public DbSet<Characteristic> Characteristics { get; set; }

        public DbSet<ObjectType> ObjectTypes { get; set; }

        public DbSet<BaseCharacteristicValue> CharacteristicValues { get; set; }

        public DbSet<CharacteristicAlias> CharacteristicAliases { get; set; }

        public DbSet<HierarchyObjectAlias> HierarchyObjectAliases { get; set; }

        INestedSetsObjectContext<HierarchyObject> IApplicationDbContext.HierarchyObjectContext => this._objectDbSetManager;

        INestedSetsFSContext<Characteristic> IApplicationDbContext.CharacteristicContext => this._characteristicDbSetManager;

        INestedSetsFSContext<ObjectType> IApplicationDbContext.ObjectTypeContext => this._objectTypeDbSetManager;

        ICharacteristicValueDbSetManager IApplicationDbContext.CharacteristicValueDbSetManager => this._characteristicValueDbSetManager;

        ICharacteristicAliasDbSetManager IApplicationDbContext.CharacteristicAliasDbSetManager => this._characteristicAliasDbSetManager;

        IHierarchyObjectAliasDbSetManager IApplicationDbContext.HierarchyObjectAliasDbSetManager => this._hierarchyObjectAliasDbSetManager;
    }
}
 