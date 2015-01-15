using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SwaggerAPIDocumentation;
using SwaggerAPIDocumentation.Interfaces;

namespace Wb.Controllers
{
    public class SwaggerController : Controller
    {
        private readonly ISwaggerApiDocumentation _apiDocumentation;

        public SwaggerController()
        {
            _apiDocumentation = new SwaggerApiDocumentation<GetSearchableConceptsController>();
        }

        [ActionName("Index")]
        public ActionResult Index(String name)
        {
            return !String.IsNullOrEmpty(name) ? this.Get(name) : this.Get();
        }

        public ActionResult Get()
        {
            return Content(_apiDocumentation.GetSwaggerApiList() , "application/json");
        }

        public ActionResult Get(string name)
        {
            var controllerType =
            Type.GetType("Wb.Controllers."+name+"Controller");
            return Content(_apiDocumentation.GetControllerDocumentation(controllerType, "http: //example.com") ,
            "application/json" )
            ;
        }
    }
}

