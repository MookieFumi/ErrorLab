using System;
using System.Web;

namespace ErrorLab.WebUI.Infrastructure
{
    public static class ElmahUtilities
    {
        //No queremos registrar en Elmah las excepciones 404 ni las AppException
        public static bool Dismiss(Exception e)
        {
            var baseException = e.GetBaseException();
            if (baseException is HttpException)
            {
                var httpCode = ((HttpException)baseException).GetHttpCode();
                if (httpCode == 404)
                {
                    return true;
                }
            }

            if (baseException is AppException)
            {
                return true;
            }

            return false;
        }
    }
}