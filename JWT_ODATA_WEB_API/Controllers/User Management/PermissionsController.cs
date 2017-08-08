using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using JWT_ODATA_WEB_API.DataAccess;
using JWT_ODATA_WEB_API.Infrastructure;
using JWT_ODATA_WEB_API.Infrastructure.Attributes;
using JWT_ODATA_WEB_API.Migrations.Seeds;
using JWT_ODATA_WEB_API.Models;

namespace JWT_ODATA_WEB_API.Controllers.User_Management
{
    public class PermissionsController : ODataController
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [EnableQuery]
        [PermissionAuthorization(Permission = Permissions.ViewUsers)]
        public IQueryable<Permission> GetPermissions()
        {
            return db.Permissions;
        }

        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public IHttpActionResult Post(Permission permission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Permissions.Add(permission);

            try
            {
                db.SaveChanges();
                return Created(permission);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            var userPermission = db.Permissions.Find(key);
            if (userPermission == null)
            {
                return NotFound();
            }

            db.Permissions.Remove(userPermission);
            db.SaveChanges();

            return Delete(key);
        }
    }
}