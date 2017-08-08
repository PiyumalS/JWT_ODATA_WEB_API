#region

using System;
using JWT_ODATA_WEB_API.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

#endregion

namespace JWT_ODATA_WEB_API.Migrations.Seeds
{
    public class Users : ISeed
    {
        public void SeedData(ApplicationDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = new ApplicationUser();


            var superPowerUser = new ApplicationUser
            {
                UserName = "Piyumal",
                Email = "piyumalrc@gmail.com",
                EmailConfirmed = true,
                FirstName = "Piyumal",
                LastName = "Somathilake",
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-3)
            };

            manager.Create(superPowerUser, "pss123");
            user = manager.FindByName("Piyumal");
            manager.AddToRoles(user.Id, Roles.SuperAdmin);

        }
    }
}