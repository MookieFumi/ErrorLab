using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ErrorLab.WebUI.Infrastructure
{
    internal static class ApplicationUtilities
    {
        internal static bool IsCustomErrorEnabled(bool treatRemoteOnlyLikeEnabled)
        {
            var customErrorsSection = (CustomErrorsSection)ConfigurationManager.GetSection("system.web/customErrors");
            CustomErrorsMode mode = customErrorsSection.Mode;
            bool retval = false;
            switch (mode)
            {
                case CustomErrorsMode.RemoteOnly:
                    if (!treatRemoteOnlyLikeEnabled)
                    {
                        retval = !HttpContext.Current.Request.IsLocal;
                    }
                    else
                    {
                        retval = true;
                    }
                    break;
                case CustomErrorsMode.On:
                    retval = true;
                    break;
                case CustomErrorsMode.Off:
                    retval = false;
                    break;
            }
            return retval;
        }
    }
}