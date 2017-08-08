using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using JWT_ODATA_WEB_API.DataAccess;
using JWT_ODATA_WEB_API.Infrastructure;
using JWT_ODATA_WEB_API.Infrastructure.Attributes;
using JWT_ODATA_WEB_API.Migrations.Seeds;
using JWT_ODATA_WEB_API.Models;

namespace JWT_ODATA_WEB_API.Controllers.User_Management
{
    public class PermissionRoleMapsController : ODataController
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/PermissionRoleMaps
        [EnableQuery]
        [PermissionAuthorization(Permission = Permissions.ViewUsers)]
        public IQueryable<PermissionRoleMap> GetPermissionRoleMaps()
        {
            return db.PermissionRoleMap;
        }

        // GET: odata/PermissionRoleMaps(5)
        [EnableQuery]
        [PermissionAuthorization(Permission = Permissions.ViewUsers)]
        public SingleResult<PermissionRoleMap> GetPermissionRoleMap([FromODataUri] int key)
        {
            return SingleResult.Create(db.PermissionRoleMap.Where(permissionRoleMap => permissionRoleMap.Id == key));
        }

        // PUT: odata/PermissionRoleMaps(5)
        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<PermissionRoleMap> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permissionRoleMap = await db.PermissionRoleMap.FindAsync(key);
            if (permissionRoleMap == null)
            {
                return NotFound();
            }

            patch.Put(permissionRoleMap);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermissionRoleMapExists(key))
                {
                    return NotFound();
                }
                throw;
            }

            return Updated(permissionRoleMap);
        }

        // POST: odata/PermissionRoleMaps
        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public async Task<IHttpActionResult> Post(PermissionRoleMap permissionRoleMap)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PermissionRoleMap.Add(permissionRoleMap);
            await db.SaveChangesAsync();

            return Created(permissionRoleMap);
        }

        // PATCH: odata/PermissionRoleMaps(5)
        [AcceptVerbs("PATCH", "MERGE")]
        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<PermissionRoleMap> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permissionRoleMap = await db.PermissionRoleMap.FindAsync(key);
            if (permissionRoleMap == null)
            {
                return NotFound();
            }

            patch.Patch(permissionRoleMap);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermissionRoleMapExists(key))
                {
                    return NotFound();
                }
                throw;
            }

            return Updated(permissionRoleMap);
        }

        // DELETE: odata/PermissionRoleMaps(5)
        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var permissionRoleMap = await db.PermissionRoleMap.FindAsync(key);
            if (permissionRoleMap == null)
            {
                return NotFound();
            }

            db.PermissionRoleMap.Remove(permissionRoleMap);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/PermissionRoleMaps(5)/Permission
        [EnableQuery]
        public SingleResult<Permission> GetPermission([FromODataUri] int key)
        {
            return SingleResult.Create(db.PermissionRoleMap.Where(m => m.Id == key).Select(m => m.Permission));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PermissionRoleMapExists(int key)
        {
            return db.PermissionRoleMap.Count(e => e.Id == key) > 0;
        }

        [HttpPost]
        [PermissionAuthorization(Permission = Permissions.WriteUsers)]
        public IHttpActionResult PostPermissionRoleMaps(ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var permissionRoleMap = parameters["permissionRoleMap"] as IEnumerable<PermissionRoleMap>;
            if (permissionRoleMap != null)
            {
                var roleId = permissionRoleMap.FirstOrDefault()?.RoleId;
                //delete exsiting roles
                foreach (var role in db.PermissionRoleMap.Where(c => c.RoleId == roleId).ToList())
                {
                    db.PermissionRoleMap.Remove(role);
                }

                // add the roles in the map
                if (permissionRoleMap != null && permissionRoleMap.Any())
                {
                    foreach (var roleMap in permissionRoleMap)
                    {
                        db.PermissionRoleMap.Add(roleMap);
                    }
                    db.SaveChanges();
                    return Updated(permissionRoleMap);
                }
            }

            return Created(permissionRoleMap);
        }
    }
}