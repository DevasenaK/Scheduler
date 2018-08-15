using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using Employee_Scheduler.Models.Repository;
using Employee_Scheduler.Service;
using Unity.Lifetime;
using Employee_Scheduler.App_Start;
using Unity.AspNet.WebApi;


namespace Employee_Scheduler
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();
           container.RegisterType<ISchedulerService, SchedulerService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISchedulerService, SchedulerService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEngineerRepository, EngineerRepository>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new UnityDependencyResolver(container);

           // UnityConfig.ConfigureUnity(config);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}"
                //defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
