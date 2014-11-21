using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Elmah;
using ErrorLab.WebUI;
using ErrorLab.WebUI.Infrastructure;

namespace ErrorLab.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            if (ElmahUtilities.Dismiss(e.Exception))
            {
                e.Dismiss();
            }
        }

        private void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            if (ElmahUtilities.Dismiss(e.Exception))
            {
                e.Dismiss();
            }
        }
    }
}
