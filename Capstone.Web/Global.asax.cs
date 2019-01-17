using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Web.Common.WebHost;
using Capstone.Web.Database;
using Capstone.Web.Mock;
using System.Configuration;

namespace Capstone.Web
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            // Bind Database
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            kernel.Bind<IMealDBService>().To<MealDBService>().WithConstructorArgument("connectionString", connectionString);
            //kernel.Bind<IMealDBService>().To<MockMealDBService>().WithConstructorArgument("connectionString", connectionString);

            return kernel;
        }
    }
}
