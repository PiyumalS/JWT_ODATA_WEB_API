using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
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
    public class UsersController : ODataController
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [EnableQuery]
        [PermissionAuthorization(Permission = Permissions.ViewUsers)]
        public IQueryable<ApplicationUser> GetUsers()
        {
            return db.Users;
        }


        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public IHttpActionResult Post(ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);


            if (userManager.FindByName(applicationUser.UserName) == null)
            {
                var unHashedPassword = applicationUser.PasswordHash;
                applicationUser.JoinDate = DateTime.Now;
                applicationUser.PasswordHash = null;
                applicationUser.EmailConfirmed = true;
                userManager.Create(applicationUser, unHashedPassword);

                var ctx = store.Context;
                ctx.SaveChanges();

                return Created(applicationUser);
            }
            return Conflict();
        }


        [PermissionAuthorization]
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<ApplicationUser> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Validate(patch.GetEntity());

            var applicationUser = db.Users.Find(key);
            if (applicationUser == null)
            {
                return NotFound();
            }

            //old password hash
            var passwordHash = applicationUser.PasswordHash;


            patch.Patch(applicationUser);

            if (!string.IsNullOrEmpty(applicationUser.PasswordHash))
            {
                var userMan =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var newPw = applicationUser.PasswordHash;
                userMan.RemovePassword(applicationUser.Id);
                userMan.AddPassword(applicationUser.Id, applicationUser.PasswordHash);
                applicationUser.PasswordHash = userMan.PasswordHasher.HashPassword(applicationUser.PasswordHash);
            }
            else
            {
                applicationUser.PasswordHash = passwordHash;
            }


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return InternalServerError(exception);
            }

            return Updated(applicationUser);
        }


        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            var applicationUser = db.Users.Find(key);
            if (applicationUser == null)
            {
                return NotFound();
            }

            db.Users.Remove(applicationUser);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }


        [PermissionAuthorization(Permission = Permissions.ViewUsers)]
        [EnableQuery]
        [PermissionAuthorization(Permission = Permissions.ViewUsers)]
        public IQueryable<ApplicationUserRole> GetRoles([FromODataUri] string key)
        {
            var db = new ApplicationDbContext();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);

            var roles = userManager.FindById(key).Roles;

            var result = from r in roles
                join x in db.Roles on r.RoleId equals x.Id
                select x;
            return result.Select(cty => new ApplicationUserRole {Id = cty.Id, Name = cty.Name}).AsQueryable();
        }
    }
}