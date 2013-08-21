using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryMgr.Models.Abstract
{
    public interface IInventoryRepository
    {
        InventoryItem Get(int id);
        IQueryable<InventoryItem> GetAll(int id);
        InventoryItem Add(InventoryItem item);
        InventoryItem Update(InventoryItem item);
        void Delete(int itemId);
        IEnumerable<InventoryItem> GetByCategory(InventoryCategory category);
        IEnumerable<InventoryItem> GetByMeasurement(InventoryQtyMeasurements qtyMeasurement);
    }
}
