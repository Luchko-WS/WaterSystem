using OpenDataStorage.Core.Constants;
using System.Security.Principal;

namespace OpenDataStorage.Helpers
{
    public static class RolesHelper
    {
        public const string USERS_MANAGER_GROUP = 
            IdentityConstants.Roles.SYSADMIN_ROLE + ", " + 
            IdentityConstants.Roles.USERS_MANAGER_ROLE + ", " + 
            IdentityConstants.Roles.TECH_SUPPORT_ROLE;

        public const string DATA_MANAGEMENT_GROUP = 
            IdentityConstants.Roles.SYSADMIN_ROLE + ", " + 
            IdentityConstants.Roles.DATA_MANAGER_ROLE + ", " + 
            IdentityConstants.Roles.DATA_SYNC_MANAGER_ROLE + ", " + 
            IdentityConstants.Roles.TECH_SUPPORT_ROLE;

        public const string DATA_SYNC_GROUP = 
            IdentityConstants.Roles.SYSADMIN_ROLE + ", " + 
            IdentityConstants.Roles.DATA_SYNC_MANAGER_ROLE;

        public const string READ_GROUP =
            IdentityConstants.Roles.SYSADMIN_ROLE + ", " +
            IdentityConstants.Roles.DATA_MANAGER_ROLE + ", " +
            IdentityConstants.Roles.DATA_SYNC_MANAGER_ROLE + ", " +
            IdentityConstants.Roles.TECH_SUPPORT_ROLE + ", " +
            IdentityConstants.Roles.USERS_MANAGER_ROLE + ", " +
            IdentityConstants.Roles.USER_ROLE;

        public static bool IsManagerGroup(IPrincipal user)
        {
            return
                user.IsInRole(IdentityConstants.Roles.SYSADMIN_ROLE) ||
                user.IsInRole(IdentityConstants.Roles.USERS_MANAGER_ROLE) ||
                user.IsInRole(IdentityConstants.Roles.DATA_MANAGER_ROLE) ||
                user.IsInRole(IdentityConstants.Roles.DATA_SYNC_MANAGER_ROLE) ||
                user.IsInRole(IdentityConstants.Roles.TECH_SUPPORT_ROLE);
        }

        public static bool IsInUsersManagerGroup(IPrincipal user)
        {
            return
                user.IsInRole(IdentityConstants.Roles.SYSADMIN_ROLE) ||
                user.IsInRole(IdentityConstants.Roles.USERS_MANAGER_ROLE) ||
                user.IsInRole(IdentityConstants.Roles.TECH_SUPPORT_ROLE);
        }

        public static bool IsInDataManagerGroup(IPrincipal user)
        {
            return
                user.IsInRole(IdentityConstants.Roles.SYSADMIN_ROLE) ||
                user.IsInRole(IdentityConstants.Roles.DATA_MANAGER_ROLE) ||
                user.IsInRole(IdentityConstants.Roles.TECH_SUPPORT_ROLE);
        }

        public static bool IsInDataSyncGroup(IPrincipal user)
        {
            return
                user.IsInRole(IdentityConstants.Roles.SYSADMIN_ROLE) ||
                user.IsInRole(IdentityConstants.Roles.DATA_SYNC_MANAGER_ROLE);
        }
    }
}