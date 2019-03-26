using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenDataStorageCore;

namespace OpenDataStorage.Common.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext 
    {
        private HierarchyObjectDbContextManager _objectDbContextManager;
        private CharacteristicDbContextManager _characteristicDbContextManager;

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            _objectDbContextManager = new HierarchyObjectDbContextManager(this, HierarchyObjects);
            _characteristicDbContextManager = new CharacteristicDbContextManager(this, Characteristics);
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

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<HierarchyObject> HierarchyObjects { get; set; }

        public DbSet<Characteristic> Characteristics { get; set; }

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

        /*
#region NestedSets

#region HierarchyObjects

public Task AddNode(NestedSetsObject parent)
{
throw new NotImplementedException();

//$right_ key – правый ключ родительского узла, $level – уровень родительского узла, либо 0, если родительского нет.

UPDATE my_tree SET left_key = left_key + 2, right_ key = right_ key + 2 WHERE left_key > $right_ key
UPDATE my_tree SET right_key = right_key + 2 WHERE right_key >= $right_key AND left_key < $right_key
INSERT INTO my_tree SET left_key = $right_key, right_key = $right_key + 1, level = $level + 1
UPDATE my_tree SET right_key = right_key + 2, left_key = IF(left_key > $right_key, left_key + 2, left_key) WHERE right_key >= $right_key

}

public Task RemoveNode(NestedSetsObject node)
{
//$left_key – левый ключ удаляемого узла, а $right_key – правый

DELETE FROM my_tree WHERE left_key >= $left_key AND right_ key <= $right_key
UPDATE my_tree SET right_key = right_key – ($right_key - $left_key + 1)***WHERE right_key > $right_key AND left_key < $left_key
UPDATE my_tree SET left_key = left_key – ($right_key - $left_key + 1), right_key = right_key – ($right_key - $left_key + 1) WHERE left_key > $right_key
UPDATE my_tree SET left_key = IF(left_key > $left_key, left_key – ($right_key - $left_key + 1), left_key), right_key = right_key – ($right_key - $left_key + 1) WHERE right_key > $right_key

throw new NotImplementedException();
}

public Task MoveNode(NestedSetsObject newParent, NestedSetsObject oldParent)
{

$groupRightKey - новий ключ
$groupLevel = новий левел 

$oldObjectRightKey - старий правий ключ

UPDATE object_tree SET left_key=left_key+2, right_key=right_key+2 WHERE left_key > $groupRightKey;";

//оновлюємо нову батьківську гілку
UPDATE object_tree SET right_key=right_key+2 WHERE right_key >= $groupRightKey AND left_key < $groupRightKey;";

//оновлюємо новий вузол
UPDATE object_tree SET left_key = $groupRightKey, right_key = ".($groupRightKey + 1), node_level = ".($groupLevel + 1)." WHERE id = $objectID;";

//оновлюємо ключі від попереднього вузла
UPDATE object_tree SET right_key=right_key-2 WHERE right_key > $oldObjectRightKey;
UPDATE object_tree SET left_key=left_key-2 WHERE left_key > $oldObjectRightKey;

throw new NotImplementedException();
}

public Task<ICollection<NestedSetsObject>> GetChildNodes(NestedSetsObject node)
{
throw new NotImplementedException();


SELECT id, name, level FROM my_tree WHERE left_key >= $left_key AND right_key <= $right_key ORDER BY left_key

}

public Task<string> GetPath(NestedSetsObject destinationNode)
{
throw new NotImplementedException();


SELECT id, name, level FROM my_tree WHERE left_key <= $left_key AND right_key >= $right_key ORDER BY left_key

}

public Task<NestedSetsObject> GetRootdNode(NestedSetsObject node)
{
throw new NotImplementedException();


SELECT id, name, level FROM my_tree WHERE left_key <= $left_key AND right_key >= $right_key AND level = $level - 1 ORDER BY left_key

}

public Task<ICollection<NestedSetsObject>> GetRootNodes(NestedSetsObject node)
{
throw new NotImplementedException();


SELECT id, name, level FROM my_tree WHERE left_key <= $left_key AND right_key >= $right_key ORDER BY left_key

}

#endregion
*/
    }
}
 