#region

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using JWT_ODATA_WEB_API.Infrastructure;
using JWT_ODATA_WEB_API.Migrations;
using JWT_ODATA_WEB_API.Utils;
using Newtonsoft.Json;

#endregion

namespace JWT_ODATA_WEB_API.Controllers.Db
{
    [RoutePrefix("api/Seeder")]
    public class SeedController : ApiController
    {
        [Route("Seed")]
        [HttpGet]
        public async Task<IHttpActionResult> Seed()
        {
            try
            {
                var db = new ApplicationDbContext();


                var dbMigrator = new DbMigrator(new Configuration());
                dbMigrator.Update();

                return Ok("Seeding Completed.");

            }
            catch (Exception exception)
            {
                return InternalServerError(exception.InnerException);
            }
        }
    }
}