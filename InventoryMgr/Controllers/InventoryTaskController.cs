using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventoryMgr.Models.Abstract;
using InventoryMgr.Models;
using InventoryMgr.Filters;
using WebMatrix.WebData;

namespace InventoryMgr.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class InventoryTaskController : Controller
    {

        private IInventoryTaskItemRepository _iTaskRepo { get; set; }
        public ITaskListRepository _iTaskListsRepo { get; set; }
        private IInventoryRepository _iInventoryRepo { get; set; }

        public InventoryTaskController(IInventoryTaskItemRepository iTaskRepo, 
            IInventoryRepository invRepo, ITaskListRepository iTaskListsRepo)
        {
            _iTaskRepo = iTaskRepo;
            _iInventoryRepo = invRepo;
            _iTaskListsRepo = iTaskListsRepo;
        }

        //
        // GET: /InventoryTask/Details/5

        public ActionResult Details(int id)
        {
            InventoryTaskItem iTask = _iTaskRepo.Get(id);
            if (iTask != null)
            {
                ViewBag.InventoryItem = _iInventoryRepo.Get(iTask.InventoryItemId);
                return PartialView("_Details", iTask);
            }
            return HttpNotFound();
        }

        //
        // GET: /InventoryTask/Create

        public ActionResult Create(int id)
        {
            int userId = WebSecurity.CurrentUserId;
            ViewBag.UserId = userId;
            ViewBag.InventoryItemsCount = _iInventoryRepo.GetAll(userId).Count();
            ViewBag.InventoryItems = _iInventoryRepo.GetAll(userId).ToSelectListItems();
            ViewBag.TaskListId = id;
            return PartialView("_Create");
        }
                
        //
        // GET: /InventoryTask/Edit/5

        public ActionResult Edit(int id)
        {
            InventoryTaskItem iTask = _iTaskRepo.Get(id);
            if (iTask == null)
            {
                return HttpNotFound();
            }

            int userId = WebSecurity.CurrentUserId;
            ViewBag.InventoryItems = _iInventoryRepo.GetAll(userId).ToSelectListItems(iTask.InventoryItemId);
            ViewBag.TaskLists = _iTaskListsRepo.GetAll(userId).ToSelectListItems(iTask.TaskListId);
            return PartialView("_Edit", iTask);
        }        
    }
}
