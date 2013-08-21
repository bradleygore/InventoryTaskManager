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
    public class InventoryTasksController : ApiController
    {
        private IInventoryTaskItemRepository _iTaskRepo { get; set; }
        private IInventoryRepository _invRepo { get; set; }
        private ITaskListRepository _taskListsRepo { get; set; }

        public InventoryTasksController(IInventoryTaskItemRepository iTaskRepo, IInventoryRepository invRepo,
            ITaskListRepository taskListsRepo)
        {
            _iTaskRepo = iTaskRepo;
            _invRepo = invRepo;
            _taskListsRepo = taskListsRepo;
        }

        // POST api/inventorytasks
        [AcceptVerbs("POST")]
        public HttpResponseMessage Post([FromBody]InventoryTaskItem iTask)
        {
            if (ModelState.IsValid)
            {
                iTask = _iTaskRepo.Add(iTask);
                HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.Created, iTask);
                msg.Headers.Location = new Uri(Request.RequestUri, String.Format("InventoryTask/Edit/{0}", iTask.Id));
                return msg;
            }
            //Else
            return Request.CreateResponse(HttpStatusCode.BadRequest, iTask);
        }

        // PUT api/inventorytasks/5
        [AcceptVerbs("PUT")]
        public HttpResponseMessage Put([FromBody]InventoryTaskItem iTask)
        {
            iTask = _iTaskRepo.Update(iTask);
            return Request.CreateResponse(HttpStatusCode.OK, iTask);
        }

        // DELETE api/inventorytasks/5
        public HttpResponseMessage Delete(int id)
        {
            _iTaskRepo.Delete(id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
