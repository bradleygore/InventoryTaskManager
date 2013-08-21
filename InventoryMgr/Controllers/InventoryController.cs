using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using InventoryMgr.Filters;
using InventoryMgr.Models.Abstract;
using InventoryMgr.Models;
using AutoMapper;
using InventoryMgr.Models.ViewModels;

namespace InventoryMgr.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class InventoryController : Controller
    {
        private ICategoriesRepository _catRepos { get; set; }
        private IQtyMeasurementsRepository _qtyMeasurementsRepos { get; set; }
        private IInventoryRepository _inventoryRepos { get; set; }

        public InventoryController(ICategoriesRepository catsRepo,
            IQtyMeasurementsRepository qrepo,
            IInventoryRepository invRepos)
        {
            _catRepos = catsRepo;
            _qtyMeasurementsRepos = qrepo;
            _inventoryRepos = invRepos;
        }
        //
        // GET: /Inventory/
        public ActionResult Index()
        {
            int userId = WebSecurity.CurrentUserId;
            ViewBag.UserId = userId;
            //ViewBag.Categories = _catRepos.GetAll(userId).ToSelectListItems();
            //ViewBag.Measurements = _qtyMeasurementsRepos.GetAll(userId).ToSelectListItems();
            ViewBag.Title = "My Inventory";
            return View();
        }

        //
        //GET: /Inventory/Create
        public ActionResult Create() {
            int userId = WebSecurity.CurrentUserId;
            ViewBag.UserId = userId;
            ViewBag.Categories = _catRepos.GetAll(userId).ToSelectListItems();
            ViewBag.Measurements = _qtyMeasurementsRepos.GetAll(userId).ToSelectListItems();
            return PartialView("_Create", new InventoryItem());
        }

        //
        //Get: /Inventory/Edit/id 
        public ActionResult Edit(int Id)
        {           
            var invItem = _inventoryRepos.Get(Id);
            InventoryItemEditModel editModel = Mapper.Map<InventoryItem, InventoryItemEditModel>(invItem);
            //editModel.CategoryId = invItem.InventoryCategory.Id;
            //editModel.MeasurementsId = invItem.InventoryQtyMeasurements.Id;
            editModel.CategoryList = _catRepos.GetAll(invItem.UserId).ToSelectListItems(invItem.InventoryCategory.Id);
            editModel.MeasurementsList = _qtyMeasurementsRepos.GetAll(invItem.UserId).ToSelectListItems(invItem.InventoryQtyMeasurements.Id);


            //View with a View Model
            return PartialView("_Edit", editModel);
            
            //View without a View Model
            //return PartialView("_Edit", invItem);
        }

    }
}
