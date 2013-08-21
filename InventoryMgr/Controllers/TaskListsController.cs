using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InventoryMgr.Models;
using InventoryMgr.Models.Abstract;

namespace InventoryMgr.Controllers
{
    public class TaskListsController : ApiController
    {
        private ITaskListRepository _tlRepo { get; set; }
        private IInventoryTaskItemRepository _iTaskRepo { get; set; }
        private ITaskItemRepository _genTaskRepo { get; set; }
        private IInventoryRepository _invRepo { get; set; }

        public TaskListsController(ITaskListRepository iTLRepo, IInventoryTaskItemRepository iTaskRepo, IInventoryRepository invRepo, ITaskItemRepository taskRepo)
        {
            _tlRepo = iTLRepo;
            _iTaskRepo = iTaskRepo;
            _invRepo = invRepo;
            _genTaskRepo = taskRepo;
        }

        // GET api/tasklists/5  NOTE: This will be used to get task lists for a specific individual
        public HttpResponseMessage GetAll(int id)
        {
            var taskItems = _tlRepo.GetAll(id).OrderBy(t => t.TaskName);
            if (taskItems != null)
                return Request.CreateResponse(HttpStatusCode.OK, taskItems);

            //Else
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        //
        // GET: /TaskList/Details/5

        public HttpResponseMessage Details(int id)
        {
            TaskList tasklist = _tlRepo.Get(id);
            if (tasklist != null)
                return Request.CreateResponse(HttpStatusCode.OK, tasklist);
            //Else
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST api/tasklists
        [AcceptVerbs("POST")]
        public HttpResponseMessage Post([FromBody]TaskList newTL)
        {
            if (ModelState.IsValid)
            {
                newTL = _tlRepo.Add(newTL);
                HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.Created, newTL);
                msg.Headers.Location = new Uri(Request.RequestUri, String.Format("TaskList/Edit/{0}", newTL.Id));
                return msg;
            }
            //Else
            return Request.CreateResponse(HttpStatusCode.BadRequest, newTL);
        }

        // PUT api/tasklists
        [AcceptVerbs("PUT")]
        public HttpResponseMessage Put([FromBody]TaskList newTL)
        {
            //Need to handle batch Completes and Un-Completes
            TaskList _tl = _tlRepo.Get(newTL.Id);

            if (newTL.IsComplete && !_tl.IsComplete)
            {
                //This whole list was just now completed - want to complete all tasks that are left on this task list that haven't been completed yet.
                //Inventory Tasks
                _iTaskRepo.GetIncomplete(newTL.UserId).Where(t => t.TaskListId == newTL.Id).ToList().ForEach(t =>
                    {
                        t.Completed = true;
                        var invItem = _invRepo.Get(t.InventoryItemId);
                        invItem.ItemQuantity += t.Quantity;
                        _invRepo.Update(invItem);
                        _iTaskRepo.Update(t);
                    });
                //General Tasks
                _genTaskRepo.GetIncomplete(newTL.UserId).Where(t => t.TaskListId == newTL.Id && !t.Completed).ToList().ForEach(t =>
                    {
                        t.Completed = true;
                        _genTaskRepo.Update(t);
                    });
            }

            _tl.IsComplete = newTL.IsComplete;

            newTL = _tlRepo.Update(_tl);
            return Request.CreateResponse(HttpStatusCode.OK, newTL);
        }

        // DELETE api/tasklists/5
        public HttpResponseMessage Delete(int Id)
        {
            _tlRepo.Delete(Id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
