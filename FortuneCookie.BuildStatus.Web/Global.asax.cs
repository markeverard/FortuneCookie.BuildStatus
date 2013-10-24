using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using FortuneCookie.Aspects;
using FortuneCookie.BuildStatus.Domain;
using FortuneCookie.BuildStatus.Domain.BuildDataServices;
using FortuneCookie.BuildStatus.Domain.BuildStatusProviders;
using FortuneCookie.BuildStatus.Ioc;
using StructureMap;

namespace FortuneCookie.BuildStatus.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            IContainer container = new Container(x => { 
                x.For<IControllerActivator>().Use<StructureMapControllerActivator>();
                x.For<IBuildStatusProvider>().Use<CompositeBuildStatusProvider>().Ctor<IEnumerable<IBuildStatusProvider>>("buildStatusProviders").Is(CompositeScreenScraperProviderFactory());
            });
            
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
        }

        private IEnumerable<IBuildStatusProvider> CompositeScreenScraperProviderFactory()
        {
            //data service - picked from Google's 1st secrah result. Change the paths to be your CC.Net dashboard path 
            var serviceDetails = new DataServiceDetails("1st Google result for ViewFarmReport.aspx", "http://build.nauck-it.de/ViewFarmReport.aspx", "http://build.nauck-it.de/ViewFarmReport.aspx");
            
            //Set up screen scraper build status provider
            var dataService = new AspectInterceptorFactory().CreateInstance<HttpDataService>(new object[] {serviceDetails});
         
            return new List<IBuildStatusProvider>
                                   {
                                       new BuildStatusProvider(dataService),
                                   };
        }
    }

}