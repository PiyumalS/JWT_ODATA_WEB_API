using JWT_ODATA_WEB_API.Infrastructure;
using JWT_ODATA_WEB_API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JWT_ODATA_WEB_API.Migrations.Seeds
{
    public class PermissionRoleMaps : ISeed
    {
        public void SeedData(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            #region Super Admin

            var superAdminRole = roleManager.FindByName(Roles.SuperAdmin);

            foreach (var permission in context.Permissions)
            {
                context.PermissionRoleMap.Add(new PermissionRoleMap
                {
                    RoleId = superAdminRole.Id,
                    PermissionId = permission.Id
                });
            }

            #endregion

            context.SaveChanges();
        }
    }
}