using System.Web.Http;
using JWT_ODATA_WEB_API.App_Start;
using Microsoft.Owin.Cors;
using Owin;
using ServiceStack.Text;

namespace JWT_ODATA_WEB_API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();

            AuthConfiguration.ConfigureOAuthTokenGeneration(app);

            AuthConfiguration.ConfigureOAuthTokenConsumption(app);

            OdataConfiguration.Register(httpConfig);

            app.UseCors(CorsOptions.AllowAll);

            JsConfig.EmitCamelCaseNames = true;

            WebApiConfigurations.ConfigureWebApi(httpConfig);

            httpConfig.EnsureInitialized();
        }
    }
}