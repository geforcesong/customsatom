using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomsAtomMobileSite.Models;

namespace CustomsAtomMobileSite.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string startDate, string endDate)
        {            
            DeclarationSearchResult result = new DeclarationSearchResult();
            DateTime startDateTime;
            if (!DateTime.TryParse(startDate, out startDateTime))
                startDateTime = DateTime.Now;
            DateTime endDateTime;
            if (!DateTime.TryParse(endDate, out endDateTime))
                endDateTime = DateTime.Now;
            int id =(int) Session["LoginID"];
            string filter = string.Format("d.receiveddate >= '{0}' AND d.receiveddate <= '{1}'", startDateTime.ToString(), endDateTime.ToString());

            result.Declarations = Declaration.GetDeclarations(id, filter);
            return View(result);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
