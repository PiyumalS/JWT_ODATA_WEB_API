using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using JWT_ODATA_WEB_API.Infrastructure;
using JWT_ODATA_WEB_API.Infrastructure.Attributes;
using JWT_ODATA_WEB_API.Migrations.Seeds;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JWT_ODATA_WEB_API.Controllers.User_Management
{
    public class UserRoleMapsController : ODataController
    {
        [HttpPost]
        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public IHttpActionResult Post(UserRoleMap userRoleMap)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);

            var result = userManager.AddToRole(userRoleMap.UserId, userRoleMap.RoleId);

            if (result.Succeeded)
            {
                return Created(userRoleMap);
            }

            return InternalServerError();
        }

        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public IHttpActionResult Delete([FromBody] UserRoleMap key2)
        {
            var roleMap = new UserRoleMap();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);

            var result = userManager.RemoveFromRole(roleMap.UserId, roleMap.RoleId);

            if (result.Succeeded)
            {
                return Created(key2);
            }

            return InternalServerError();
        }

        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public async Task<IHttpActionResult> PostUserRoleMaps(ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var superAdminRole = roleManager.FindByName("SuperAdmin");
            var userRoleMaps = parameters["userRoleMaps"] as IEnumerable<UserRoleMap>;
            if (userRoleMaps != null)
            {
                var userId = userRoleMaps.FirstOrDefault()?.UserId;
                //delete exsiting roles
                foreach (var role in userManager.GetRoles(userId))
                {
                    userManager.RemoveFromRoles(userId, role);
                }

                // add the roles in the map
                if (userRoleMaps != null && userRoleMaps.Any())
                {
                    foreach (var roleMap in userRoleMaps)
                    {
                        userManager.AddToRole(userId, roleManager.FindById(roleMap.RoleId).Name);
                    }

                    return Updated(userRoleMaps);
                }
            }

            return Created(userRoleMaps);
        }
    }
}