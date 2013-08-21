using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryMgr.Models.Abstract;

namespace InventoryMgr.Models.Repos
{
    public class TaskListRepository : ITaskListRepository
    {
        private InventoryContext _db { get; set; }

        public TaskListRepository()
            : this(new InventoryContext())
        {

        }

        public TaskListRepository(InventoryContext db)
        {
            _db = db;
        }

        public TaskList Get(int id)
        {
            return _db.TaskLists.Include("Tasks").SingleOrDefault(t => t.Id == id);
        }

        public IEnumerable<TaskList> GetAll(int userId)
        {
            var allTaskLists = _db.TaskLists.Include("Tasks").Where(l => l.UserId == userId);
            return allTaskLists;
        }

        public IEnumerable<TaskList> GetIncomplete(int userId)
        {
            return _db.TaskLists.Where(l => l.UserId == userId && !l.IsComplete);
        }

        public IEnumerable<TaskList> GetByDate(DateTime taskDate, int userId)
        {
            return _db.TaskLists.Include("TaskItems").Where(l => l.UserId == userId);
        }

        public TaskList Add(TaskList taskList)
        {
            taskList = _db.TaskLists.Add(taskList);
            _db.SaveChanges();

            return taskList;
        }

        public TaskList Update(TaskList taskList)
        {
            _db.Entry(taskList).State = System.Data.EntityState.Modified;
            _db.SaveChanges();
            return taskList;
        }

        public void Delete(int id)
        {
            var taskList = _db.TaskLists.SingleOrDefault(t => t.Id == id);
            if (taskList != null)
            {
                _db.TaskLists.Remove(taskList);
                _db.SaveChanges();
            }
        }
    }
}