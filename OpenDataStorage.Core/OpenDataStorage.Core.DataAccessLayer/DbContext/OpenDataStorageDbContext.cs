using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenDataStorage.Core.DataAccessLayer.Common;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.BaseEntityDbSetManagers.Aliases;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.BaseEntityDbSetManagers.CharacteristicValues;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers;
using OpenDataStorage.Core.DataAccessLayer.DbSetManagers.NestedSetsEntityManagers.Core;
using OpenDataStorage.Core.Entities;
using OpenDataStorage.Core.Entities.Aliases;
using OpenDataStorage.Core.Entities.CharacteristicValues;
using OpenDataStorage.Core.Entities.NestedSets;

namespace OpenDataStorage.Core.DataAccessLayer.DbContext
{
    public class OpenDataStorageDbContext : IdentityDbContext<ApplicationUser>, IOpenDataStorageDbContext, IDbContainer
    {
        private INestedSetsDbSetManager<HierarchyObject> _objectDbSetManager;
        private INestedSetsDbSetManager<Characteristic> _characteristicDbSetManager;
        private INestedSetsDbSetManager<ObjectType> _objectTypeDbSetManager;
        private ICharacteristicValueDbSetManager _characteristicValueDbSetManager;
        private IAliasDbSetManager<CharacteristicAlias> _characteristicAliasDbSetManager;
        private IAliasDbSetManager<HierarchyObjectAlias> _hierarchyObjectAliasDbSetManager;

        public OpenDataStorageDbContext(string nameOrConnectionString, bool throwIfV1Schema)
            : base(nameOrConnectionString, throwIfV1Schema)
        {
            InitManagers();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<OpenDataStorageDbContext, DbInitializer>());
            Configuration.ProxyCreationEnabled = false;
        }

        public async Task ReloadEntityFromDb(object entity)
        {
            await this.Entry(entity).ReloadAsync();
        }

        public DbContextTransaction BeginTransaction()
        {
            return this.Database.BeginTransaction();
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
        }

        private void InitManagers()
        {
            _objectDbSetManager = new HierarchyObjectDbSetManager(HierarchyObjects, this);
            _characteristicDbSetManager = new CharacteristicDbSetManager(Characteristics, this);
            _objectTypeDbSetManager = new ObjectTypeDbSetManager(ObjectTypes, this);
            _characteristicValueDbSetManager = new CharacteristicValueDbSetManager<BaseCharacteristicValue>(CharacteristicValues, this);
            _characteristicAliasDbSetManager = new CharacteristicAliasDbSetManager(CharacteristicAliases, this);
            _hierarchyObjectAliasDbSetManager = new HierarchyObjectAliasDbSetManager(HierarchyObjectAliases, this);
        }

        async Task IDbContextBase.SaveDbChangesAsync()
        {
            await this.SaveChangesAsync();
        }

        public DbSet<HierarchyObject> HierarchyObjects { get; set; }

        public DbSet<Characteristic> Characteristics { get; set; }

        public DbSet<ObjectType> ObjectTypes { get; set; }

        public DbSet<BaseCharacteristicValue> CharacteristicValues { get; set; }

        public DbSet<CharacteristicAlias> CharacteristicAliases { get; set; }

        public DbSet<HierarchyObjectAlias> HierarchyObjectAliases { get; set; }

        INestedSetsDbSetManager<HierarchyObject> IOpenDataStorageDbContext.HierarchyObjectContext => this._objectDbSetManager;

        INestedSetsDbSetManager<Characteristic> IOpenDataStorageDbContext.CharacteristicContext => this._characteristicDbSetManager;

        INestedSetsDbSetManager<ObjectType> IOpenDataStorageDbContext.ObjectTypeContext => this._objectTypeDbSetManager;

        ICharacteristicValueDbSetManager IOpenDataStorageDbContext.CharacteristicValueDbSetManager => this._characteristicValueDbSetManager;

        IAliasDbSetManager<CharacteristicAlias> IOpenDataStorageDbContext.CharacteristicAliasDbSetManager => this._characteristicAliasDbSetManager;

        IAliasDbSetManager<HierarchyObjectAlias> IOpenDataStorageDbContext.HierarchyObjectAliasDbSetManager => this._hierarchyObjectAliasDbSetManager;
    }
}
 