using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventoryMgr.Models;
using InventoryMgr.Models.Abstract;

namespace InventoryMgr.Controllers
{
    [Authorize]
    public class MeasurementController : Controller
    {
        private IQtyMeasurementsRepository _qtys { get; set; }

        public MeasurementController(IQtyMeasurementsRepository qtysRepo)
        {
            _qtys = qtysRepo;
        }

        //
        // GET: /Quantity/Edit/5
        public ActionResult Edit(int id)
        {
            var qty = _qtys.Get(id);
            return PartialView("_Edit", qty);
        }
    }
}
