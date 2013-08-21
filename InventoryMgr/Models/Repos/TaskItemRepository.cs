using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryMgr.Models.Abstract;

namespace InventoryMgr.Models.Repos
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private InventoryContext _db { get; set; }

        public TaskItemRepository()
            : this(new InventoryContext())
        {

        }

        public TaskItemRepository(InventoryContext db)
        {
            _db = db;
        }

        public TaskItem Get(int id)
        {
            return _db.TaskItems.SingleOrDefault(t => t.Id == id);
        }

        public IEnumerable<TaskItem> Get(DateTime taskDate, int userId)
        {
            return _db.TaskItems.Where(t => t.UserId == userId && t.TaskDate == taskDate);
        }

        public IEnumerable<TaskItem> GetIncomplete(int userId)
        {
            return _db.TaskItems.Where(t => t.UserId == userId && !(t.Completed));
        }

        public IEnumerable<TaskItem> GetIncomplete(DateTime taskDate, int userId)
        {
            return _db.TaskItems.Where(t => t.UserId == userId && !(t.Completed) && t.TaskDate == taskDate);
        }

        public IEnumerable<TaskItem> GetComplete(int userId)
        {
            return _db.TaskItems.Where(t => t.UserId == userId && t.Completed);
        }

        public TaskItem Add(TaskItem ti)
        {
            _db.TaskItems.Add(ti);
            _db.SaveChanges();
            return ti;
        }

        //Set or remove the completed date on the task item itself
        public TaskItem Update(TaskItem ti)
        {
            if (ti.Completed && String.IsNullOrEmpty(ti.CompletedDate.ToString()))
                ti.CompletedDate = DateTime.Now;

            if (!ti.Completed && !String.IsNullOrEmpty(ti.CompletedDate.ToString()))
	            ti.CompletedDate = null;


            _db.Entry(ti).State = System.Data.EntityState.Modified;
            _db.SaveChanges();
            return ti;
        }

        public void Delete(int id)
        {
            TaskItem taskItem = _db.TaskItems.SingleOrDefault(t => t.Id == id);
            if (taskItem != null)
            {
                _db.TaskItems.Remove(taskItem);
                _db.SaveChanges(); 
            }
        }
    }
}