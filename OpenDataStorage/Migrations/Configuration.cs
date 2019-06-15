namespace OpenDataStorage.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using OpenDataStorage.Common.DbContext;
    using OpenDataStorage.Helpers;
    using OpenDataStorageCore.Constants;
    using OpenDataStorageCore.Entities;
    using OpenDataStorageCore.Entities.NestedSets;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            PrepateStoredProceduresForCharacteristics(context);
            PrepateStoredProceduresForHierarchyObjects(context);
            PrepateStoredProceduresForObjectsTypes(context);

            AddConstraintsToHierarchyObjectsTable(context);

            CreateUserRoles(context);
            CreateAdminUser(context);
            CreateRootObjects(context);
        }

        private void CreateRootObjects(ApplicationDbContext context)
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

        private void CreateUserRoles(ApplicationDbContext context)
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

        private void CreateAdminUser(ApplicationDbContext context)
        {
            if (!context.Users.Any(u => u.UserName == IdentityConstants.Admin.USER_NAME))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = new ApplicationUser
                {
                    UserName = IdentityConstants.Admin.USER_NAME,
                    Email = "net@net.net",
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

        private void AddConstraintsToHierarchyObjectsTable(ApplicationDbContext context)
        {
            var hierarchyObjectsTableName = ((IApplicationDbContext)context).HierarchyObjectContext.TableName;
            var typesTableName = ((IApplicationDbContext)context).ObjectTypeContext.TableName;

            var primaryKey = ReflectionHelper.GetPropName((ObjectType t) => t.Id);
            var foreignKey = ReflectionHelper.GetPropName((HierarchyObject o) => o.ObjectTypeId);
            var constraintName = string.Format("FK_dbo.{0}_dbo.{1}_{2}", hierarchyObjectsTableName, typesTableName, foreignKey);

            context.Database.ExecuteSqlCommand($"ALTER TABLE [dbo].[{hierarchyObjectsTableName}] DROP CONSTRAINT [{constraintName}]");
            context.Database.ExecuteSqlCommand($"ALTER TABLE [dbo].[{hierarchyObjectsTableName}] ADD CONSTRAINT [{constraintName}] FOREIGN KEY ([{foreignKey}]) REFERENCES [dbo].[{typesTableName}] ([{primaryKey}]) ON DELETE SET NULL");
        }

        private void PrepateStoredProceduresForHierarchyObjects(ApplicationDbContext context)
        {
            string tableName = ((IApplicationDbContext)context).HierarchyObjectContext.TableName;
            CreateStoredProcedurePreCreateNestedSetsNode(context, tableName);
            CreateStoredProcedurePostRemoveNestedSetsNode(context, tableName);
            CreateStoredProcedureMoveNestedSetsNode(context, tableName);
        }

        private void PrepateStoredProceduresForCharacteristics(ApplicationDbContext context)
        {
            string tableName = ((IApplicationDbContext)context).CharacteristicContext.TableName;
            CreateStoredProcedurePreCreateNestedSetsNode(context, tableName);
            CreateStoredProcedurePostRemoveNestedSetsNode(context, tableName);
            CreateStoredProcedureMoveNestedSetsNode(context, tableName);
        }

        private void PrepateStoredProceduresForObjectsTypes(ApplicationDbContext context)
        {
            string tableName = ((IApplicationDbContext)context).ObjectTypeContext.TableName;
            CreateStoredProcedurePreCreateNestedSetsNode(context, tableName);
            CreateStoredProcedurePostRemoveNestedSetsNode(context, tableName);
            CreateStoredProcedureMoveNestedSetsNode(context, tableName);
        }

        private void CreateStoredProcedurePreCreateNestedSetsNode(ApplicationDbContext context, string tableName)
        {
            string procedureName = string.Format("{0}PreCreateNestedSetsNode", tableName);

            context.Database.ExecuteSqlCommand(string.Format(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[{0}]", procedureName));

            context.Database.ExecuteSqlCommand(string.Format(@"CREATE PROCEDURE [dbo].[{0}]
                @RightKey int
            AS
            BEGIN
                UPDATE dbo.[{1}] SET LeftKey = LeftKey + 2, RightKey = RightKey + 2 WHERE LeftKey > @RightKey;
                UPDATE dbo.[{1}] SET RightKey = RightKey + 2 WHERE RightKey >= @RightKey AND LeftKey < @RightKey;
            END", procedureName, tableName));
        }

        private void CreateStoredProcedurePostRemoveNestedSetsNode(ApplicationDbContext context, string tableName)
        {
            string procedureName = string.Format("{0}PostRemoveNestedSetsNode", tableName);

            context.Database.ExecuteSqlCommand(string.Format(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[{0}]", procedureName));

            context.Database.ExecuteSqlCommand(string.Format(@"CREATE PROCEDURE [dbo].[{0}]
                @LeftKey int,
                @RightKey int
            AS
            BEGIN
                UPDATE dbo.[{1}] SET RightKey = RightKey - (@RightKey - @LeftKey + 1) WHERE RightKey > @RightKey AND LeftKey < @LeftKey;
                UPDATE dbo.[{1}] SET LeftKey = LeftKey - (@RightKey - @LeftKey + 1), RightKey = RightKey - (@RightKey - @LeftKey + 1) WHERE LeftKey > @RightKey;
            END", procedureName, tableName));
        }

        private void CreateStoredProcedureMoveNestedSetsNode(ApplicationDbContext context, string tableName)
        {
            string procedureName = string.Format("{0}MoveNestedSetsNode", tableName);

            context.Database.ExecuteSqlCommand(string.Format(@"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[{0}]", procedureName));

            context.Database.ExecuteSqlCommand(string.Format(@"CREATE PROCEDURE [dbo].[{0}]
                @NodeLeftKey int,
                @NodeRightKey int,
                @NodeLevel int,
                @ParentLeftKey int,
                @ParentRightKey int,
                @ParentLevel int
            AS
            BEGIN
                DECLARE @d int = @NodeRightKey - @NodeLeftKey + 1;
                UPDATE dbo.[{1}] SET LeftKey = 0 - LeftKey, RightKey = 0 - RightKey WHERE LeftKey >= @NodeLeftKey AND RightKey <= @NodeRightKey;
                UPDATE dbo.[{1}] SET LeftKey = LeftKey - @d WHERE LeftKey > @NodeRightKey;
                UPDATE dbo.[{1}] SET RightKey = RightKey - @d WHERE RightKey > @NodeRightKey;

                DECLARE @pr int = CASE WHEN @ParentRightKey > @NodeRightKey THEN @ParentRightKey - (@NodeRightKey - @NodeLeftKey + 1) ELSE @ParentRightKey END;
                UPDATE dbo.[{1}] SET LeftKey = LeftKey + @d WHERE LeftKey >= @pr;
                UPDATE dbo.[{1}] SET RightKey = RightKey + @d WHERE RightKey >= @pr;

                DECLARE @pd int = CASE WHEN @ParentRightKey > @NodeRightKey THEN @ParentRightKey - @NodeRightKey - 1 ELSE @ParentRightKey - @NodeRightKey - 1 + @d END;
                DECLARE @dl int = @ParentLevel + 1 - @NodeLevel;
                UPDATE dbo.[{1}] SET LeftKey = @pd - LeftKey, RightKey = @pd - RightKey, Level = Level + @dl WHERE LeftKey <= 0-@NodeLeftKey AND RightKey >= 0-@NodeRightKey;
            END", procedureName, tableName));
        }
    }
}
