using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp;
using iTextSharp.text;

namespace AlfrescoWorklog.Controllers
{
    public class PdfCreatorController : Controller
    {        

        public ActionResult Index()
        {
            return View();
        }

    }
}
