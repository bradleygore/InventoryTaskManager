using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryMgr.Models.Abstract
{
    public interface ITaskListRepository
    {
        TaskList Get(int id);
        IEnumerable<TaskList> GetAll(int userId);
        IEnumerable<TaskList> GetIncomplete(int userId);
        IEnumerable<TaskList> GetByDate(DateTime taskDate, int userId);
        TaskList Add(TaskList taskList);
        TaskList Update(TaskList taskList);
        void Delete(int id);
    }
}
