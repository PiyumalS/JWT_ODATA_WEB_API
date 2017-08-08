using JWT_ODATA_WEB_API.Infrastructure;

namespace JWT_ODATA_WEB_API.Migrations.Seeds
{
    internal interface ISeed
    {
        void SeedData(ApplicationDbContext context);
    }
}