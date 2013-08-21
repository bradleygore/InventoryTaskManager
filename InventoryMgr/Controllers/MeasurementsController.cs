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
    public class MeasurementsController : ApiController
    {
        private IQtyMeasurementsRepository _qtys { get; set; }

        public MeasurementsController(IQtyMeasurementsRepository qtysRepo)
        {
            _qtys = qtysRepo;
        }

        // GET api/quantities
        public HttpResponseMessage Get(int Id)
        {
            var allQtys = _qtys.GetAll(Id).OrderBy(q => q.UnitOfMeasure);
            if (allQtys != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, allQtys);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // GET api/quantities/5
        //NOTE: No need to do this here, as we're using the non-API controller to return a partial view

        // POST api/quantities
        [AcceptVerbs("POST")]
        public HttpResponseMessage Post([FromBody]InventoryQtyMeasurements newQty)
        {
            if (ModelState.IsValid)
            {
                newQty = _qtys.Add(newQty);
                HttpResponseMessage returnMsg = Request.CreateResponse(HttpStatusCode.Created, newQty);
                returnMsg.Headers.Location = new Uri(Request.RequestUri, String.Format("Quantity/Edit/{0}", newQty.Id));
                return returnMsg;
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, newQty);
        }

        // PUT api/quantities/5
        [AcceptVerbs("PUT")]
        public HttpResponseMessage Put([FromBody]InventoryQtyMeasurements qty)
        {
            qty = _qtys.Update(qty);
            return Request.CreateResponse(HttpStatusCode.OK, qty);
        }

        // DELETE api/quantities/5
        public HttpResponseMessage Delete(int id)
        {
            _qtys.Delete(id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
