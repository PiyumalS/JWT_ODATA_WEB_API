using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Batch;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.UI.WebControls.WebParts;
using JWT_ODATA_WEB_API.Infrastructure;
using JWT_ODATA_WEB_API.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JWT_ODATA_WEB_API.App_Start
{
    public static class OdataConfiguration
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            // Web API configuration and services
            var builder = new ODataConventionModelBuilder();

            config.EnableUnqualifiedNameCall(true);

            config.EnableEnumPrefixFree(true);

            RegisterEntities(ref builder);

            RegisterCustomRoutes(ref builder);

            builder.Namespace = "";

            builder.EnableLowerCamelCase();

            ODataBatchHandler odataBatchHandler =
                new DefaultODataBatchHandler(new HttpServer(config));

            odataBatchHandler.MessageQuotas.MaxOperationsPerChangeset = 10;
            odataBatchHandler.MessageQuotas.MaxPartsPerBatch = 10;

            config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel(), odataBatchHandler);

            // Web API routes
        }

        public static void RegisterEntities(ref ODataConventionModelBuilder builder)
        {
            builder.ContainerName = "Db";

            #region User Mgt

            builder.EntitySet<ApplicationUser>("Users");
            builder.EntitySet<Permission>("Permissions");
            builder.EntitySet<IdentityUserClaim>("IdentityUserClaims");
            builder.EntitySet<UserRole>("UserRoles");
            builder.EntitySet<UserRoleMap>("UserRoleMaps");
            builder.EntitySet<PermissionRoleMap>("PermissionRoleMaps");

            #endregion

        }

        public static void RegisterCustomRoutes(ref ODataConventionModelBuilder builder)
        {
            var checkOutManyAction = builder.EntityType<UserRoleMap>().Collection.Action("PostUserRoleMaps");
            checkOutManyAction.CollectionEntityParameter<UserRoleMap>("userRoleMaps");

            var postRolePermissonMap =
                builder.EntityType<PermissionRoleMap>().Collection.Action("PostPermissionRoleMaps");
            postRolePermissonMap.CollectionEntityParameter<PermissionRoleMap>("permissionRoleMap");

        }
    }
}