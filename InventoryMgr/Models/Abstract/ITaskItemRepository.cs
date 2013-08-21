using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryMgr.Models.Abstract
{
    public interface ITaskItemRepository
    {
        TaskItem Get(int id);
        IEnumerable<TaskItem> Get(DateTime taskDate, int userId);
        IEnumerable<TaskItem> GetIncomplete(int userId);
        IEnumerable<TaskItem> GetIncomplete(DateTime taskDate, int userId);
        IEnumerable<TaskItem> GetComplete(int userId);
        TaskItem Add(TaskItem taskItem);
        TaskItem Update(TaskItem taskItem);
        void Delete(int id);
    }
}
