using Microsoft.AspNet.Identity.EntityFramework;
using OpenDataStorage.Common.Attributes;
using OpenDataStorage.Helpers;
using OpenDataStorage.ViewModels.AccountViewModels;
using OpenDataStorageCore;
using OpenDataStorageCore.Constants;
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
        [Route("GetUsersList/{skip}/{take}")]
        public async Task<dynamic> GetUsersList()
        {
            return await UserManager.Users
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
                            user.EmailConfirmed,
                            user.IsLocked,
                            user.RegisteredDate,
                            user.LastLoginTime,
                            user.Roles
                        }).ToListAsync();
        }

        [HttpPost]
        [Route("SetLockState")]
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

        [HttpPost]
        [Route("SaveUserDetails")]
        public async Task<HttpResponseMessage> SaveUserDetails(ApplicationUserViewModel vm)
        {
            if (!string.IsNullOrEmpty(vm?.UserName))
            {
                var user = await UserManager.FindByNameAsync(vm.UserName);
                if (user != null)
                {
                    if (await IsLowerPermission(user))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You have not permission");
                    }
                    //add code here
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User is missing");
        }

        public async Task<HttpResponseMessage> DeleteUser(string userName)
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
                    var res = await UserManager.DeleteAsync(user);
                    if (res.Succeeded)
                    {
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Failed to delete user.");
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User is missing");
        }

        /*[Route("ChangePasswordForUser")]
        [HttpPost]
        public async Task<HttpResponseMessage> ChangePasswordForUser(ChangePasswordForUserViewModel model)
        {
            var user = await UserManager.FindByIdAsync(model.UserId);
            if (user != null)
            {
                var token = await UserManager.GeneratePasswordResetTokenAsync(model.UserId);
                var result = await UserManager.ResetPasswordAsync(model.UserId, token, model.NewPassword);
                if (result.Succeeded)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Failed to change password for user " + model.UserId);
        }*/

        private async Task<bool> IsLowerPermission(ApplicationUser user)
        {
            var adminRole = await RoleManager.FindByNameAsync(IdentityConstants.Roles.SYSADMIN_ROLE);
            var currentUser = await GetCurrentUser();
            return (!adminRole.Users.Any(u => u.UserId == currentUser.Id) && adminRole.Users.Any(u => u.UserId == user.Id));
        }
    }
}