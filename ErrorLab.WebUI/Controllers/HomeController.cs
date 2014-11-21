using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using ErrorLab.WebUI.Infrastructure;
using Newtonsoft.Json;

namespace ErrorLab.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            try
            {
                throw new AppException("Es un error de lógica de negocio.");
            }
            catch (AppException e)
            {

                ViewBag.Error = e.Message;
            }

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            try
            {
                throw new AppException("Contact Exception");
            }
            catch (AppException e)
            {
                ViewBag.Exception = e.Message;
            }


            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult InvokeAppException()
        {
            throw new AppException("Es un error de lógica de negocio.");
        }

        [HttpPost]
        public ActionResult InvokeError()
        {
            throw new IdentityNotMappedException();
        }

        [HttpPost]
        public ActionResult Invoke()
        {
            return new EmptyResult();
        }
    }
}