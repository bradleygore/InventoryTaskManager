using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventoryMgr.Filters;
using InventoryMgr.Models.Abstract;
using InventoryMgr.Models;

namespace InventoryMgr.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private ICategoriesRepository _catRepos { get; set; }
        
        public CategoryController(ICategoriesRepository catsRepo,
            IQtyMeasurementsRepository qrepo)
        {
            _catRepos = catsRepo;
        }

        //Get: /category/Edit/5
        public ActionResult Edit(int id)
        {
            var cat = _catRepos.Get(id);
            return PartialView("_Edit", cat);
        }
    }
}
