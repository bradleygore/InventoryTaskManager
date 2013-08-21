using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InventoryMgr.Models.Abstract;
using InventoryMgr.Models;

namespace InventoryMgr.Controllers
{
    public class GeneralTasksController : ApiController
    {
        private ITaskItemRepository _iTaskRepo { get; set; }
        private ITaskListRepository _taskListsRepo { get; set; }

        public GeneralTasksController(ITaskItemRepository iTaskRepo, ITaskListRepository taskListsRepo)
        {
            _iTaskRepo = iTaskRepo;
            _taskListsRepo = taskListsRepo;
        }


        // POST api/regulartasks
        [AcceptVerbs("POST")]
        public HttpResponseMessage Post([FromBody]TaskItem task)
        {
            if (ModelState.IsValid)
            {
                task = _iTaskRepo.Add(task);
                HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.Created, task);
                msg.Headers.Location = new Uri(Request.RequestUri, String.Format("RegularTask/Edit/{0}", task.Id));
                return msg;
            }
            //Else
            return Request.CreateResponse(HttpStatusCode.BadRequest, task);
        }

        // PUT api/regulartasks/5
        [AcceptVerbs("PUT")]
        public HttpResponseMessage Put([FromBody]TaskItem task)
        {
            task = _iTaskRepo.Update(task);
            return Request.CreateResponse(HttpStatusCode.OK, task);
        }

        // DELETE api/regulartasks/5
        public HttpResponseMessage Delete(int id)
        {
            _iTaskRepo.Delete(id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
