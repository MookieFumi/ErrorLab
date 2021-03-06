﻿using System;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Elmah;

namespace ErrorLab.WebUI.Infrastructure
{
    public class HandleErrorWithElmahAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var e = context.Exception;
            var httpContext = context.HttpContext;

            if ((e is AppException)
                && ApplicationUtilities.IsCustomErrorEnabled(true)
                && httpContext.Request.IsAjaxRequest())
            {
                var response = httpContext.Response;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.ContentType = "text/plain";
                response.Write(e.Message);
                context.ExceptionHandled = true;
                response.TrySkipIisCustomErrors = true;
                return;
            }
            base.OnException(context);
            if (!context.ExceptionHandled
                || RaiseErrorSignal(e)
                || IsFiltered(context))
            {
                return;
            }
            LogException(e);
        }

        private static bool IsFiltered(ExceptionContext context)
        {
            var config = context.HttpContext.GetSection("elmah/errorFilter") as ErrorFilterConfiguration;
            if (config == null)
                return false;
            var testContext = new ErrorFilterModule.AssertionHelperContext(context.Exception, HttpContext.Current);
            return config.Assertion.Test(testContext);
        }

        private static void LogException(Exception e)
        {
            if (ElmahUtilities.Dismiss(e))
            {
                return;
            }
            var context = HttpContext.Current;
            ErrorLog.GetDefault(context).Log(new Error(e, context));
        }

        private static bool RaiseErrorSignal(Exception e)
        {
            var context = HttpContext.Current;
            if (context == null)
                return false;
            var signal = ErrorSignal.FromContext(context);
            if (signal == null)
                return false;
            signal.Raise(e, context);
            return true;
        }
    }
}