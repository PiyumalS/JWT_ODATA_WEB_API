using System.Linq;
using JWT_ODATA_WEB_API.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JWT_ODATA_WEB_API.Migrations.Seeds
{
    public class Roles : ISeed
    {
        public static string Admin = "Admin";
        public static string NetworkAdmin = "NetworkAdmin";
        public static string SuperAdmin = "SuperAdmin";

        public void SeedData(ApplicationDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole {Name = Admin});

                roleManager.Create(new IdentityRole {Name = SuperAdmin});

                roleManager.Create(new IdentityRole {Name = NetworkAdmin});

            }

            context.SaveChanges();
        }
    }
}