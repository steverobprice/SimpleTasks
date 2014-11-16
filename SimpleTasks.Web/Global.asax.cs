using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper;
using SimpleTasks.Data;
using SimpleTasks.Data.Repositories;
using SimpleTasks.Services;
using SimpleTasks.Services.MappingProfiles;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SimpleTasks.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register the all controllers.
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register other dependencies.
            builder.Register(c => new TasksDbContext()).As<TasksDbContext>().InstancePerRequest();
            builder.RegisterGeneric(typeof(Repository<>)).AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(TaskService).Assembly)
                .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerRequest();

            // Build the container.
            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new TaskProfile());
            });
        }
    }
}
