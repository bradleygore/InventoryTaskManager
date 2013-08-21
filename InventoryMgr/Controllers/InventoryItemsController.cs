using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InventoryMgr.Models;
using InventoryMgr.Models.Abstract;
using InventoryMgr.Models.Repos;
using InventoryMgr.Models.ViewModels;
using AutoMapper;

namespace InventoryMgr.Controllers
{
    public class InventoryItemsController : ApiController
    {
        private ICategoriesRepository _catRepos { get; set; }
        private IQtyMeasurementsRepository _qtyMeasurementsRepos { get; set; }
        private IInventoryRepository _inventoryRepos { get; set; }
        
        public InventoryItemsController(ICategoriesRepository catsRepo, 
            IQtyMeasurementsRepository qRepo, 
            IInventoryRepository invRepos)
        {
            _catRepos = catsRepo;
            _qtyMeasurementsRepos = qRepo;
            _inventoryRepos = invRepos;
        }

        //GET api/InventoryItems
        public HttpResponseMessage GetAll(int userId)
        {
            var items = _inventoryRepos.GetAll(userId).OrderBy(i => i.InventoryCategory.CategoryName).ThenBy(i => i.ItemName);
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        //NOTE: No need to get a single inventoryItem here, as we'll do that in the non-API InventoryController that can return a partial view.
        //GET: api/InventoryItems/5...


        //POST api/InventoryItems
        public HttpResponseMessage Post(InventoryItem item)
        {            
            try
            {
                item = _inventoryRepos.Add(item);
                var response = Request.CreateResponse(HttpStatusCode.Created, item);
                //Get the URL to retrieve the newly created item
                response.Headers.Location = new Uri(Request.RequestUri, String.Format("InventoryItems/GetSingle/{0}", item.Id));

                return response;
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, item);
            }
        }

        #region PUT// Before using a View Model
        /*NOT USING VIEW MODEL*/
        //PUT api/InventoryItems/
        //[AcceptVerbs("PUT")]
        //public HttpResponseMessage Put(InventoryItem item)
        //{
        //    item.ItemLastUpdated = DateTime.Now;
        //    item.InventoryCategory = _catRepos.Get(item.InventoryCategory.Id);
        //    item.InventoryQtyMeasurements = _qtyMeasurementsRepos.Get(item.InventoryQtyMeasurements.Id);
        //    _inventoryRepos.Update(item);
        //    return Request.CreateResponse(HttpStatusCode.OK, item);
        //}
        #endregion Before using a View Model

        /*USING VIEW MODEL*/
        //PUT api/InventoryItems/
        [AcceptVerbs("PUT")]
        public HttpResponseMessage Put(InventoryItemEditModel item)
        {
            InventoryItem updatedItem = Mapper.Map<InventoryItemEditModel, InventoryItem>(item);
            updatedItem.InventoryCategory = _catRepos.Get(item.InventoryCategoryId);
            updatedItem.InventoryQtyMeasurements = _qtyMeasurementsRepos.Get(item.InventoryQtyMeasurementsId);
            _inventoryRepos.Update(updatedItem);

            return Request.CreateResponse(HttpStatusCode.OK, item);
        }


        //DELETE api/InventoryItems/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            _inventoryRepos.Delete(id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
