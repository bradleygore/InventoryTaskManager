using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventoryMgr.Models.Abstract;
using InventoryMgr.Filters;
using InventoryMgr.Models;
using WebMatrix.WebData;

namespace InventoryMgr.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class GeneralTaskController : Controller
    {
        private ITaskItemRepository _iTaskRepo { get; set; }
        private ITaskListRepository _taskListsRepo { get; set; }

        public GeneralTaskController(ITaskItemRepository iTaskRepo, ITaskListRepository taskListsRepo)
        {
            _iTaskRepo = iTaskRepo;
            _taskListsRepo = taskListsRepo;
        }


        //
        // GET: /RegularTask/Details/5

        public ActionResult Details(int id)
        {
            TaskItem item = _iTaskRepo.Get(id);
            if (item != null)
            {
                return PartialView("_Details", item);
            }

            return HttpNotFound();
        }

        //
        // GET: /RegularTask/Create

        public ActionResult Create(int id)
        {
            int userId = WebSecurity.CurrentUserId;
            ViewBag.UserId = userId;
            ViewBag.TaskListId = id;
            return PartialView("_Create");
        }

        //
        // GET: /RegularTask/Edit/5

        public ActionResult Edit(int id)
        {
            TaskItem item = _iTaskRepo.Get(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            int userId = WebSecurity.CurrentUserId;
            ViewBag.TaskLists = _taskListsRepo.GetAll(userId).ToSelectListItems(item.TaskListId);
            return PartialView("_Edit", item);
        }
    }
}
