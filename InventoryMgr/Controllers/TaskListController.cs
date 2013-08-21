using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventoryMgr.Models;
using InventoryMgr.Filters;
using WebMatrix.WebData;
using InventoryMgr.Models.Abstract;

namespace InventoryMgr.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class TaskListController : Controller
    {
        private ITaskListRepository _tlRepo { get; set; }

        public TaskListController(ITaskListRepository iTLRepo)
        {
            _tlRepo = iTLRepo;
        }

        //
        // GET: /TaskList/

        public ActionResult Index()
        {
            int userId = WebSecurity.CurrentUserId;
            ViewBag.UserId = userId;
            ViewBag.Title = "My Task Lists";
            return View();
        }

        //
        // GET: /TaskList/Create

        public ActionResult Create()
        {
            return PartialView("_Create");
        }


        //
        // GET: /TaskList/Edit/5

        public ActionResult Edit(int id)
        {
            TaskList tasklist = _tlRepo.Get(id);
            if (tasklist == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", tasklist);
        }
    }
}