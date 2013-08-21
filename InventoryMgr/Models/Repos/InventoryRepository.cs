using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryMgr.Models.Abstract;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Configuration;


namespace InventoryMgr.Models.Repos
{
    public class InventoryRepository : IInventoryRepository
    {
        private InventoryContext _db { get; set; }

        public InventoryRepository()
            : this(new InventoryContext())
        {

        }

        public InventoryRepository(InventoryContext db)
        {
            _db = db;
        }

        public InventoryItem Get(int id)
        {
            return _db.Items.Include("InventoryCategory").Include("InventoryQtyMeasurements").Single(i => i.Id == id);
        }

        public IQueryable<InventoryItem> GetAll(int id)
        {
            return _db.Items.Include("InventoryCategory").Include("InventoryQtyMeasurements").Where(i => i.UserId == id);
        }

        public InventoryItem Add(InventoryItem item)
        {
            item.ItemLastUpdated = DateTime.Now;
            item.InventoryCategory = _db.Categories.Single(c => c.Id == item.InventoryCategoryId);
            item.InventoryQtyMeasurements = _db.QtyMeasurements.Single(m => m.Id == item.InventoryQtyMeasurementsId);
            _db.Items.Add(item);
            _db.SaveChanges();
            return item;
        }

        public InventoryItem Update(InventoryItem item)
        {
            InventoryItem currentItem = _db.Items.Single(i => i.Id == item.Id);
            currentItem.InventoryQtyMeasurements = _db.QtyMeasurements.Single(m => m.Id == item.InventoryQtyMeasurementsId);
            currentItem.InventoryCategory = _db.Categories.Single(c => c.Id == item.InventoryCategoryId);
            _db.Entry(currentItem).CurrentValues.SetValues(item);
            
            //_db.Entry(item).State = System.Data.EntityState.Modified;
            _db.SaveChanges();
            return item;

            #region Updating complex object before using a View Model
            //String sqlUpdate = "UPDATE InventoryItems SET ItemName = '" + item.ItemName + "', ItemDescription = '" + item.ItemDescription + "', " +
            //    "ItemQuantity = '" + item.ItemQuantity.ToString() + "', ItemLastUpdated = '" + DateTime.Now + "', InventoryQtyMeasurements_ID = '" + item.InventoryQtyMeasurements.Id.ToString() + "', " +
            //    "InventoryCategory_Id = '" + item.InventoryCategory.Id.ToString() + "' " +
            //    "WHERE Id = " + item.Id.ToString() + ";";

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UpdateConnection"].ConnectionString))
            //{
            //    SqlCommand cmd = con.CreateCommand();
            //    cmd.CommandText = sqlUpdate;
            //    cmd.CommandType = System.Data.CommandType.Text;
            //    con.Open();
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            #endregion Updating comples object before using a View Model
        }

        public void Delete(int id)
        {
            var item = _db.Items.Single(i => i.Id == id);
            _db.Items.Remove(item);
            _db.SaveChanges();
        }

        public IEnumerable<InventoryItem> GetByCategory(InventoryCategory cat)
        {
            return _db.Items.Include("ItemCategory").Include("QtyMeasurement").Where(i => i.InventoryCategory == cat);
        }

        public IEnumerable<InventoryItem> GetByMeasurement(InventoryQtyMeasurements qtyMeasurement)
        {
            return _db.Items.Include("ItemCategory").Include("QtyMeasurement").Where(i => i.InventoryQtyMeasurements == qtyMeasurement);
        }


    }
}