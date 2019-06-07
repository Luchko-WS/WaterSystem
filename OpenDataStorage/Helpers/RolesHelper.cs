using OpenDataStorageCore.Constants;

namespace OpenDataStorage.Helpers
{
    public static class RolesHelper
    {
        public const string USERS_MANAGER_GROUP = IdentityConstants.Roles.SYSADMIN_ROLE + ", " + IdentityConstants.Roles.USERS_MANAGER_ROLE + ", " + IdentityConstants.Roles.TECH_SUPPORT_ROLE;

        public const string DATA_MANAGEMENT_GROUP = IdentityConstants.Roles.SYSADMIN_ROLE + ", " + IdentityConstants.Roles.DATA_MANAGER_ROLE + ", " + IdentityConstants.Roles.DATA_SYNC_MANAGER_ROLE + ", " + IdentityConstants.Roles.TECH_SUPPORT_ROLE;

        public const string DATA_SYNC_GROUP = IdentityConstants.Roles.SYSADMIN_ROLE + ", " + IdentityConstants.Roles.DATA_SYNC_MANAGER_ROLE;

        public const string TECH_SUPPORT_GROUP = IdentityConstants.Roles.SYSADMIN_ROLE + ", " + IdentityConstants.Roles.USERS_MANAGER_ROLE + ", " + IdentityConstants.Roles.DATA_MANAGER_ROLE + ", " + IdentityConstants.Roles.DATA_SYNC_MANAGER_ROLE + ", " + IdentityConstants.Roles.TECH_SUPPORT_ROLE;
    }
}