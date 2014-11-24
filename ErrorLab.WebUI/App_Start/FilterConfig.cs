using System.Web;
using System.Web.Mvc;
using ErrorLab.WebUI.Infrastructure;

namespace ErrorLab.WebUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleErrorWithElmahAttribute());
        }
    }
}