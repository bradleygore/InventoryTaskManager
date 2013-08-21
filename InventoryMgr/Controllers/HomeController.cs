using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using InventoryMgr.Filters;

namespace InventoryMgr.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Inventory Management";
            ViewBag.Message = "Start managing your inventory today!";
            return View();
        }
    }
}
