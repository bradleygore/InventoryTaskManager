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
    public class CategoriesController : ApiController
    {
        private ICategoriesRepository _catRepos { get; set; }

        public CategoriesController(ICategoriesRepository catsRepo)
        {
            _catRepos = catsRepo;
        }

        // GET api/categories
        public HttpResponseMessage GetAll(int userId)
        {
            var allCategories = _catRepos.GetAll(userId).OrderBy(c => c.CategoryName);
            if (allCategories != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, allCategories);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // GET api/categories/5
        //NOTE: No need to do this here, as we're using the non-API controller to return a partial view

        // POST api/categories
        [AcceptVerbs("POST")]
        public HttpResponseMessage Post([FromBody]InventoryCategory newCat)
        {
            if (ModelState.IsValid)
            {
                newCat = _catRepos.Add(newCat);
                HttpResponseMessage returnMsg = Request.CreateResponse(HttpStatusCode.Created, newCat);
                returnMsg.Headers.Location = new Uri(Request.RequestUri, String.Format("Category/Edit/{0}", newCat.Id));
                return returnMsg;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, newCat);
            }
        }

        // PUT api/categories/5
        [AcceptVerbs("Put")]
        public HttpResponseMessage Put([FromBody]InventoryCategory cat)
        {
            cat = _catRepos.Update(cat);
            return Request.CreateResponse(HttpStatusCode.OK, cat);
        }

        // DELETE api/categories/5
        public HttpResponseMessage Delete(int id)
        {
            _catRepos.Delete(id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
