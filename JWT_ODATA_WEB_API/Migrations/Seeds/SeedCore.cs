using System.Collections.Generic;
using JWT_ODATA_WEB_API.Infrastructure;

namespace JWT_ODATA_WEB_API.Migrations.Seeds
{
    public class SeedCore
    {
        public static void RunSeeds(ApplicationDbContext context)
        {
            var seedList = new List<ISeed>();
            seedList.Add(new Roles());
            seedList.Add(new Users());
            seedList.Add(new Permissions());
            seedList.Add(new PermissionRoleMaps());

            foreach (var seedResource in seedList)
            {
               seedResource.SeedData(context);
            }
        }
    }
}