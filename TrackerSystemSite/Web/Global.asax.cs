using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using DependencyResolver.Ninject;
using BusinessLayer.DataBase;
using Newtonsoft.Json.Serialization;

namespace Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            (new DbLogic()).DbInitialization();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(NinjectConfig.CreateKernel());

            //MvcHandler.DisableMvcResponseHeader = true;

            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            //config.Formatters.JsonFormatter.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
