using System.Collections.Generic;
using JWT_ODATA_WEB_API.Infrastructure;
using JWT_ODATA_WEB_API.Models;

namespace JWT_ODATA_WEB_API.Migrations.Seeds
{
    public class Permissions : ISeed
    {
        public const string ViewUsers = "ViewUsers";
        public const string WriteUsers = "WriteUsers";
        public const string DeleteUsers = "DeleteUsers";

        public void SeedData(ApplicationDbContext context)
        {
            context.Permissions.AddRange(new List<Permission>
            {
                new Permission
                {
                    Name = ViewUsers
                },
                new Permission
                {
                    Name = WriteUsers
                },
                new Permission
                {
                    Name = DeleteUsers
                }
    });

            context.SaveChanges();
        }
    }
}