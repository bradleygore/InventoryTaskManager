using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryMgr.Models.Abstract;

namespace InventoryMgr.Models.Repos
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private InventoryContext _db { get; set; }

        public CategoriesRepository()
            : this(new InventoryContext())
        {

        }

        public CategoriesRepository(InventoryContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets a single Category by its Id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>InventoryCategory</returns>
        public InventoryCategory Get(int id)
        {
            return _db.Categories.SingleOrDefault(c => c.Id == id);
        }

        public IEnumerable<InventoryCategory> GetAll(int userId)
        {
            return _db.Categories.Where(c => c.UserId == userId || c.IsGlobal == true);
        }

        public InventoryCategory Add(InventoryCategory cat)
        {
            _db.Categories.Add(cat);
            _db.SaveChanges();
            return cat;
        }

        public InventoryCategory Update(InventoryCategory cat)
        {
            _db.Entry(cat).State = System.Data.EntityState.Modified;
            _db.SaveChanges();
            return cat;
        }

        public void Delete(int id)
        {
            var category = _db.Categories.Single(c => c.Id == id);
            _db.Categories.Remove(category);
            _db.SaveChanges();
        }

        public InventoryCategory GetByNameAndUser(String name, int userId)
        {
            return _db.Categories.SingleOrDefault(c => c.CategoryName == name && (c.UserId == userId || c.IsGlobal == true));
        }

    }
}