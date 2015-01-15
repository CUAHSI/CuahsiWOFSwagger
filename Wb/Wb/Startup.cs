using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;


[assembly: OwinStartupAttribute(typeof(Wb.Startup))]
namespace Wb
{
    public partial class Startup {

        public void Configuration(IAppBuilder app) {
            HttpConfiguration config = new HttpConfiguration();
           

            ConfigureAuth(app);
            WebApiConfig.Register(config);

          // config.EnableSwagger(c => c.SingleApiVersion("1.0", "A title for your API")).EnableSwaggerUi();

            
            app.UseStageMarker(PipelineStage.MapHandler);
 
        }
    }
}
