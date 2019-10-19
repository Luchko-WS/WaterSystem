namespace OpenDataStorage.Core.DataAccessLayer.Common
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using OpenDataStorage.Common;
    using OpenDataStorage.Core.Constants;
    using OpenDataStorage.Core.DataAccessLayer.DbContext;
    using OpenDataStorage.Core.Entities;
    using OpenDataStorage.Core.Entities.NestedSets;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class DbInitializer : DbMigrationsConfiguration<OpenDataStorageDbContext>
    {
        public DbInitializer()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(OpenDataStorageDbContext context)
        {
            AddConstraintsToHierarchyObjectsTable(context);
            CreateRootEntities(context);
            CreateNestedSetsStoredProcedures(context);
        }

        private void CreateRootEntities(OpenDataStorageDbContext context)
        {
            CreateUserRoles(context);
            CreateAdminUser(context);
            CreateRootObjects(context);
        }

        private void CreateRootObjects(OpenDataStorageDbContext context)
        {
            if (!context.Characteristics.Any(c => c.Level == 0))
            {
                context.Characteristics.Add(new Characteristic
                {
                    Level = 0,
                    LeftKey = 1,
                    RightKey = 2,
                    Name = "root",
                    OwnerId = IdentityConstants.Admin.USER_NAME,
                    EntityType = EntityType.Folder
                });
            }

            if (!context.HierarchyObjects.Any(o => o.Level == 0))
            {
                context.HierarchyObjects.Add(new HierarchyObject
                {
                    Level = 0,
                    LeftKey = 1,
                    RightKey = 2,
                    Name = "root",
                    OwnerId = IdentityConstants.Admin.USER_NAME
                });
            }

            if (!context.ObjectTypes.Any(t => t.Level == 0))
            {
                context.ObjectTypes.Add(new ObjectType
                {
                    Level = 0,
                    LeftKey = 1,
                    RightKey = 2,
                    Name = "root",
                    OwnerId = IdentityConstants.Admin.USER_NAME,
                    EntityType = EntityType.Folder
                });
            }
        }

        private void CreateUserRoles(OpenDataStorageDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (!context.Roles.Any(r => r.Name == IdentityConstants.Roles.DATA_MANAGER_ROLE))
            {
                var role = new IdentityRole { Name = IdentityConstants.Roles.DATA_MANAGER_ROLE };
                roleManager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == IdentityConstants.Roles.DATA_SYNC_MANAGER_ROLE))
            {
                var role = new IdentityRole { Name = IdentityConstants.Roles.DATA_SYNC_MANAGER_ROLE };
                roleManager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == IdentityConstants.Roles.USER_ROLE))
            {
                var role = new IdentityRole { Name = IdentityConstants.Roles.USER_ROLE };
                roleManager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == IdentityConstants.Roles.SYSADMIN_ROLE))
            {
                var role = new IdentityRole { Name = IdentityConstants.Roles.SYSADMIN_ROLE };
                roleManager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == IdentityConstants.Roles.TECH_SUPPORT_ROLE))
            {
                var role = new IdentityRole { Name = IdentityConstants.Roles.TECH_SUPPORT_ROLE };
                roleManager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == IdentityConstants.Roles.USERS_MANAGER_ROLE))
            {
                var role = new IdentityRole { Name = IdentityConstants.Roles.USERS_MANAGER_ROLE };
                roleManager.Create(role);
            }
        }

        private void CreateAdminUser(OpenDataStorageDbContext context)
        {
            if (!context.Users.Any(u => u.UserName == IdentityConstants.Admin.USER_NAME))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = new ApplicationUser
                {
                    UserName = IdentityConstants.Admin.USER_NAME,
                    Email = "admin@main.com",
                    EmailConfirmed = true,
                    RegisteredDate = DateTime.Now
                };

                IdentityResult result = userManager.Create(user, IdentityConstants.Admin.PASSWORD);
                var adminUser = userManager.FindByName(IdentityConstants.Admin.USER_NAME);
                if (result.Succeeded)
                {
                    userManager.AddToRoles(user.Id,
                        IdentityConstants.Roles.SYSADMIN_ROLE,
                        IdentityConstants.Roles.DATA_MANAGER_ROLE,
                        IdentityConstants.Roles.DATA_SYNC_MANAGER_ROLE,
                        IdentityConstants.Roles.TECH_SUPPORT_ROLE,
                        IdentityConstants.Roles.USERS_MANAGER_ROLE,
                        IdentityConstants.Roles.USER_ROLE);
                }
            }
        }

        private void AddConstraintsToHierarchyObjectsTable(OpenDataStorageDbContext context)
        {
            var hierarchyObjectsTableName = ((IOpenDataStorageDbContext)context).HierarchyObjectContext.TableName;
            var typesTableName = ((IOpenDataStorageDbContext)context).ObjectTypeContext.TableName;

            var primaryKey = ReflectionHelper.GetPropName((ObjectType t) => t.Id);
            var foreignKey = ReflectionHelper.GetPropName((HierarchyObject o) => o.ObjectTypeId);
            var constraintName = string.Format("FK_dbo.{0}_dbo.{1}_{2}", hierarchyObjectsTableName, typesTableName, foreignKey);

            context.Database.ExecuteSqlCommand($"ALTER TABLE [dbo].[{hierarchyObjectsTableName}] DROP CONSTRAINT [{constraintName}]");
            context.Database.ExecuteSqlCommand($"ALTER TABLE [dbo].[{hierarchyObjectsTableName}] ADD CONSTRAINT [{constraintName}] FOREIGN KEY ([{foreignKey}]) REFERENCES [dbo].[{typesTableName}] ([{primaryKey}]) ON DELETE SET NULL");
        }

        private void CreateNestedSetsStoredProcedures(OpenDataStorageDbContext context)
        {
            var openDateStorageContext = (IOpenDataStorageDbContext)context;
            StoredProceduresManager.Instance.CreateStoredProcedures(context, openDateStorageContext.HierarchyObjectContext);
            StoredProceduresManager.Instance.CreateStoredProcedures(context, openDateStorageContext.CharacteristicContext);
            StoredProceduresManager.Instance.CreateStoredProcedures(context, openDateStorageContext.ObjectTypeContext);
        }
    }
}
