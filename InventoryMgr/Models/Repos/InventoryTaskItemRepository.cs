using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryMgr.Models.Abstract;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace InventoryMgr.Models.Repos
{
    public class InventoryTaskItemRepository : IInventoryTaskItemRepository
    {
        private InventoryContext _db = new InventoryContext();

        public InventoryTaskItemRepository()
            : this(new InventoryContext())
        {

        }

        public InventoryTaskItemRepository(InventoryContext db)
        {
            _db = db;
        }

        public InventoryTaskItem Get(int id)
        {
            return _db.InventoryTaskItems.SingleOrDefault(inv => inv.Id == id);
        }

        public IEnumerable<InventoryTaskItem> Get(DateTime taskDate, int userId)
        {
            return _db.InventoryTaskItems.Where(inv => inv.Id == userId && inv.TaskDate == taskDate);
        }

        public IEnumerable<InventoryTaskItem> GetIncomplete(int userId)
        {
            return _db.InventoryTaskItems.Where(inv => inv.UserId == userId && !(inv.Completed));
        }

        public IEnumerable<InventoryTaskItem> GetIncomplete(DateTime taskDate, int userId)
        {
            return _db.InventoryTaskItems.Where(inv => inv.UserId == userId && !(inv.Completed) && inv.TaskDate == taskDate);
        }

        public IEnumerable<InventoryTaskItem> GetComplete(int userId)
        {
            return _db.InventoryTaskItems.Where(inv => inv.UserId == userId && inv.Completed);
        }

        public InventoryTaskItem Add(InventoryTaskItem invTaskItem)
        {
            _db.InventoryTaskItems.Add(invTaskItem);
            _db.SaveChanges();
            return invTaskItem;
        }

        public InventoryTaskItem Update(InventoryTaskItem invTaskItem)
        {            
            //Set or remove the completed date on the task item itself
            if (invTaskItem.Completed && String.IsNullOrEmpty(invTaskItem.CompletedDate.ToString()))
                invTaskItem.CompletedDate = DateTime.Now;
            
            if (!invTaskItem.Completed && !String.IsNullOrEmpty(invTaskItem.CompletedDate.ToString()))
                invTaskItem.CompletedDate = null;
            
            InventoryTaskItem iTask = _db.InventoryTaskItems.Single(i => i.Id == invTaskItem.Id);
            bool alreadyCompleted = iTask.Completed;
            //Now go ahead and update the task item itself.            
            _db.Entry(iTask).CurrentValues.SetValues(invTaskItem);

            //Need to handle for when items are completed and un-completed
            InventoryItem item = _db.Items.Include("InventoryCategory").Include("InventoryQtyMeasurements").Single(i => i.Id == invTaskItem.InventoryItemId);
            item.ItemLastUpdated = DateTime.Today;

            if (!alreadyCompleted && invTaskItem.Completed)
            {
                //Then they just completed the task in this update
                item.ItemQuantity += invTaskItem.Quantity;
            }
            else if (alreadyCompleted && !invTaskItem.Completed)
            {
                //Then they just un-completed the task in this update
                item.ItemQuantity -= invTaskItem.Quantity;
            }

            _db.SaveChanges();
            return invTaskItem;
        }

        public void Delete(int id)
        {
            InventoryTaskItem taskItem = _db.InventoryTaskItems.SingleOrDefault(t => t.Id == id);
            if (taskItem != null)
            {
                _db.TaskItems.Remove(taskItem);
                _db.SaveChanges();
            }
        }
    }
}