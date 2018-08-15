using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Employee_Scheduler.Controllers;
using Employee_Scheduler.Models;
using Employee_Scheduler.Service;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;


[assembly: OwinStartup(typeof(Employee_Scheduler.App_Start.Startup))]

namespace Employee_Scheduler.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            var services = new ServiceCollection();
            //ConfigureAuth(app);
            ConfigureServices(services);           
           

        }
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();
            //services.AddSingleton<ISchedulerService,SchedulerService>();
            //services.AddScoped<ISchedulerService, SchedulerService>();
   //         services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
   //.Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
   //.Where(t => typeof(IController).IsAssignableFrom(t)
   //   || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));

        }

    }
}
