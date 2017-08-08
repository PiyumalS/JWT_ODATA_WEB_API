using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using JWT_ODATA_WEB_API.DataAccess;
using JWT_ODATA_WEB_API.Infrastructure;
using JWT_ODATA_WEB_API.Infrastructure.Attributes;
using JWT_ODATA_WEB_API.Migrations.Seeds;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JWT_ODATA_WEB_API.Controllers.User_Management
{
    public class UserRolesController : ODataController
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [EnableQuery]
        [PermissionAuthorization(Permission = Permissions.ViewUsers)]
        public IQueryable<UserRole> Get()
        {
            return db.Roles.Select(cty => new UserRole {Id = cty.Id, Name = cty.Name}).AsQueryable();
        }

        [EnableQuery]
        [PermissionAuthorization(Permission = Permissions.ViewUsers)]
        public SingleResult<UserRole> Get([FromODataUri] string key)
        {
            return
                SingleResult.Create(
                    db.Roles.Where(userRole => userRole.Id == key)
                        .Select(cty => new UserRole {Id = cty.Id, Name = cty.Name})
                        .AsQueryable());
        }


        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public IHttpActionResult Post(UserRole userRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));


            var exists = roleManager.Roles.Any(role => role.Name == userRole.Name);

            if (exists)
                return Conflict();

            var result = roleManager.Create(new IdentityRole {Name = userRole.Name});
            if (result.Succeeded)
            {
                var content = roleManager.Roles.Where(role => role.Name == userRole.Name)
                    .Select(cty => new UserRole {Id = cty.Id, Name = cty.Name})
                    .First();

                return Created(content);
            }
            return InternalServerError();
        }

        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<UserRole> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userRole = db.Roles.Find(key);
            if (userRole == null)
            {
                return NotFound();
            }
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var role = roleManager.FindById(key);
            role.Name = patch.GetEntity().Name;
            var result = roleManager.Update(role);
            if (result.Succeeded)
            {
                return Updated(userRole);
            }
            return InternalServerError();
        }

        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            var userRole = db.Roles.Find(key);
            if (userRole == null)
            {
                return NotFound();
            }

            db.Roles.Remove(userRole);
            db.SaveChanges();

            return Updated(userRole);
        }
    }
}