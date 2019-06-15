using Microsoft.AspNet.Identity.EntityFramework;
using OpenDataStorage.Common.Attributes;
using OpenDataStorage.Helpers;
using OpenDataStorageCore.Constants;
using OpenDataStorageCore.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenDataStorage.API
{
    [RoutePrefix("api/UsersManagement")]
    [WebApiAuthorize(Roles = RolesHelper.USERS_MANAGER_GROUP)]
    public class UsersManagementController : BaseApiController
    {
        [HttpGet]
        public async Task<ApplicationUser> Get(string userName)
        {
            return await UserManager.FindByNameAsync(userName);
        }

        [HttpGet]
        [Route("GetCurrentUser")]
        [AllowAnonymous]
        public async Task<ApplicationUser> GetCurrentUser()
        {
            return await UserManager.FindByNameAsync(User.Identity.Name);
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<List<IdentityRole>> GetAllRoles()
        {
            return await RoleManager.Roles.ToListAsync();
        }

        [HttpPost]
        [Route("AddUserToRole/{userName}/{roleId}")]
        public async Task<HttpResponseMessage> AddUserToRole(string userName, string roleName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
            {
                var role = await RoleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    if (role.Users.All(u => u.UserId != user.Id))
                    {
                        var res = await UserManager.AddToRoleAsync(user.Id, role.Name);
                        if (res.Succeeded)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, user);
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, user, "User already is added to role");
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to add user to role");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Role is missing");
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User is missing");
        }

        [HttpPost]
        [Route("RemoveUserFromRole/{userName}/{roleId}")]
        public async Task<HttpResponseMessage> RemoveUserFromRole(string userName, string roleName)
        {
            var user = await UserManager.FindByNameAsync(userName);

            if (user != null)
            {
                if (await IsLowerPermission(user))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You have not permission");
                }

                var role = await RoleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    if (role.Users.Any(u => u.UserId == user.Id))
                    {
                        var res = await UserManager.RemoveFromRoleAsync(user.Id, role.Name);
                        if (res.Succeeded)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, user);
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, user, "User isn't conected to this role");
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed to remove user from role");
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Role is missing");
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User is missing");
        }

        [HttpGet]
        [Route("GetUsersList")]
        public async Task<dynamic> GetUsersList()
        {
            var res = await UserManager.Users
                .Where(u => u.UserName != IdentityConstants.Admin.USER_NAME)
                .Select(
                    user =>
                        new
                        {
                            user.Id,
                            user.UserName,
                            user.FirstName,
                            user.LastName,
                            user.Email,
                            user.IsLocked,
                            user.RegisteredDate,
                            user.LastLoginTime,
                            user.Roles
                        }).ToListAsync();
            return res;
        }

        [HttpPost]
        [Route("SetLockState/{userName}/{isLocked}")]
        public async Task<HttpResponseMessage> SetLockState(string userName, bool isLocked)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var user = await UserManager.FindByNameAsync(userName);
                if (user != null)
                {
                    if (await IsLowerPermission(user))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You have not permission");
                    }

                    user.IsLocked = isLocked;
                    var res = await UserManager.UpdateAsync(user);
                    if (res.Succeeded)
                    {
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Failed to change lock state.");
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User is missing");
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<HttpResponseMessage> DeleteUser([FromUri] Guid id)
        {
            var user = await UserManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                if (await IsLowerPermission(user))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You have not permission");
                }
                var res = await UserManager.DeleteAsync(user);
                if (res.Succeeded)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Failed to delete user.");
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User is missing");
        }

        [Route("ChangePasswordForUser")]
        [HttpPost]
        public async Task<HttpResponseMessage> ChangePasswordForUser(string userName, string newPassword)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
            {
                if (await IsLowerPermission(user))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You have not permission");
                }
                var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var result = await UserManager.ResetPasswordAsync(user.Id, token, newPassword);
                if (result.Succeeded)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Failed to change password for user.");
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User is missing");
        }

        private async Task<bool> IsLowerPermission(ApplicationUser user)
        {
            var adminRole = await RoleManager.FindByNameAsync(IdentityConstants.Roles.SYSADMIN_ROLE);
            var currentUser = await GetCurrentUser();
            return (!adminRole.Users.Any(u => u.UserId == currentUser.Id) && adminRole.Users.Any(u => u.UserId == user.Id));
        }
    }
}