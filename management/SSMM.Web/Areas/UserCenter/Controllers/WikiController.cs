using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSMM.Web.Areas.UserCenter.Controllers
{
    public class WikiController : BaseController
    {
        
        public ActionResult DownLoad()
        {
            return View();
        }

        public ActionResult Windows()
        {
            return View();
        }
        public ActionResult MacOS()
        {
            return View();
        }
        public ActionResult Android()
        {
            return View();
        }
        public ActionResult IOS()
        {
            return View();
        }
    }
}