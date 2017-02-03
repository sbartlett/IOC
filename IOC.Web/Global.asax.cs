using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IOC.Web.Controllers;
using IOC.Web.QuoteProvider;

namespace IOC.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var container = new Container();
            container.Register<IQuoteProvider, QuoteProvider.QuoteProvider>();
            container.Register<HomeController, HomeController>();

            ControllerBuilder.Current.SetControllerFactory(new IOCControllerFactory(container));
        }
    }

    public class IOCControllerFactory : DefaultControllerFactory
    {
        private readonly Container _container;

        public IOCControllerFactory(Container container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return (IController)_container.Resolve(controllerType) ;
        }
    }
}
