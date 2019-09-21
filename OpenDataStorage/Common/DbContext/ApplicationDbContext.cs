using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenDataStorage.Common.DbContext.Managers.DbSetManagers.Aliases;
using OpenDataStorage.Common.DbContext.Managers.DbSetManagers.CharacteristicValues;
using OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers;
using OpenDataStorage.Common.DbContext.Managers.NestedSetsManagers.Core;
using OpenDataStorageCore.Entities;
using OpenDataStorageCore.Entities.Aliases;
using OpenDataStorageCore.Entities.CharacteristicValues;
using OpenDataStorageCore.Entities.NestedSets;

namespace OpenDataStorage.Common.DbContext
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext, IDbContainer
    {
        private INestedSetsDbSetManager<HierarchyObject> _objectDbSetManager;
        private INestedSetsDbSetManager<Characteristic> _characteristicDbSetManager;
        private INestedSetsDbSetManager<ObjectType> _objectTypeDbSetManager;
        private ICharacteristicValueDbSetManager _characteristicValueDbSetManager;
        private IAliasDbSetManager<CharacteristicAlias> _characteristicAliasDbSetManager;
        private IAliasDbSetManager<HierarchyObjectAlias> _hierarchyObjectAliasDbSetManager;

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());
            Configuration.ProxyCreationEnabled = false;

            _objectDbSetManager = new HierarchyObjectDbSetManager(HierarchyObjects, this);
            _characteristicDbSetManager = new CharacteristicDbSetManager(Characteristics, this);
            _objectTypeDbSetManager = new ObjectTypeDbSetManager(ObjectTypes, this);
            _characteristicValueDbSetManager = new CharacteristicValueDbSetManager<BaseCharacteristicValue>(CharacteristicValues, this);
            _characteristicAliasDbSetManager = new CharacteristicAliasDbSetManager(CharacteristicAliases, this);
            _hierarchyObjectAliasDbSetManager = new HierarchyObjectAliasDbSetManager(HierarchyObjectAliases, this);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public async Task ReloadEntityFromDb(object entity)
        {
            await this.Entry(entity).ReloadAsync();
        }

        public DbContextTransaction BeginTransaction()
        {
            return this.Database.BeginTransaction();
        }

        async Task IApplicationDbContextBase.SaveDbChangesAsync()
        {
            await this.SaveChangesAsync();
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

        public DbSet<HierarchyObject> HierarchyObjects { get; set; }

        public DbSet<Characteristic> Characteristics { get; set; }

        public DbSet<ObjectType> ObjectTypes { get; set; }

        public DbSet<BaseCharacteristicValue> CharacteristicValues { get; set; }

        public DbSet<CharacteristicAlias> CharacteristicAliases { get; set; }

        public DbSet<HierarchyObjectAlias> HierarchyObjectAliases { get; set; }

        INestedSetsDbSetManager<HierarchyObject> IApplicationDbContext.HierarchyObjectContext => this._objectDbSetManager;

        INestedSetsDbSetManager<Characteristic> IApplicationDbContext.CharacteristicContext => this._characteristicDbSetManager;

        INestedSetsDbSetManager<ObjectType> IApplicationDbContext.ObjectTypeContext => this._objectTypeDbSetManager;

        ICharacteristicValueDbSetManager IApplicationDbContext.CharacteristicValueDbSetManager => this._characteristicValueDbSetManager;

        IAliasDbSetManager<CharacteristicAlias> IApplicationDbContext.CharacteristicAliasDbSetManager => this._characteristicAliasDbSetManager;

        IAliasDbSetManager<HierarchyObjectAlias> IApplicationDbContext.HierarchyObjectAliasDbSetManager => this._hierarchyObjectAliasDbSetManager;
    }
}
 