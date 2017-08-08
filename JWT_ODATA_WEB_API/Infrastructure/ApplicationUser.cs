using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JWT_ODATA_WEB_API.Infrastructure
{
    [TrackChanges]
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public byte Level { get; set; }
        public DateTime? JoinDate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            var context = new ApplicationDbContext();
            foreach (var role in Roles)
            {
                var permissions =
                    context.PermissionRoleMap.Where(c => c.RoleId == role.RoleId).Include(c => c.Permission);
                foreach (var permission in permissions)
                {
                    userIdentity.AddClaim(new Claim("permissions", permission.Permission.Name));
                }
            }
            userIdentity.AddClaim(new Claim("firstname", FirstName));
            userIdentity.AddClaim(new Claim("lastname", LastName));

            // Add custom user claims here

            return userIdentity;
        }
    }
}