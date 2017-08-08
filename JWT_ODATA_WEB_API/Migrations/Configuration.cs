using System.Data.Entity.Migrations;
using JWT_ODATA_WEB_API.Infrastructure;
using JWT_ODATA_WEB_API.Migrations.Seeds;

namespace JWT_ODATA_WEB_API.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            SeedCore.RunSeeds(context);
        }
    }
}