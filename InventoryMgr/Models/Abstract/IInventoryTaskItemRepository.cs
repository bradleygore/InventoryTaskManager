using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryMgr.Models.Abstract
{
    public interface IInventoryTaskItemRepository
    {
        InventoryTaskItem Get(int id);
        IEnumerable<InventoryTaskItem> Get(DateTime taskDate, int userId);
        IEnumerable<InventoryTaskItem> GetIncomplete(int userId);
        IEnumerable<InventoryTaskItem> GetIncomplete(DateTime taskDate, int userId);
        IEnumerable<InventoryTaskItem> GetComplete(int userId);
        InventoryTaskItem Add(InventoryTaskItem invTaskItem);
        InventoryTaskItem Update(InventoryTaskItem invTaskItem);
        void Delete(int Id);
    }
}
