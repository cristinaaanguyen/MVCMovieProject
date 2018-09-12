using AutoMapper;
using Project1.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Project1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //SETUP CODE
            Mapper.Initialize( c=> c.AddProfile<MappingProfile>()); //pass an action, when we see an action pass a lambda expression, MappingProfile type is defined in namespace AppStart
            GlobalConfiguration.Configure(WebApiConfig.Register); //need this when making API
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
