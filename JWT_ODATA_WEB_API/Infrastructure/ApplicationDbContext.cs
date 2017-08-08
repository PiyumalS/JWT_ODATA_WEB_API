using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using JWT_ODATA_WEB_API.DataAccess;
using JWT_ODATA_WEB_API.Models;
using TrackerEnabledDbContext.Identity;

namespace JWT_ODATA_WEB_API.Infrastructure
{
    public class ApplicationDbContext : TrackerIdentityContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=DefaultConnection")
        {
        }
        public DbSet<Permission> Permissions { get; set; } 
        public DbSet<PermissionRoleMap> PermissionRoleMap { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Add(new ForeignKeyNamingConvention());
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges(UserAccess.GetCurrentUserName());
        }

        
    }
}