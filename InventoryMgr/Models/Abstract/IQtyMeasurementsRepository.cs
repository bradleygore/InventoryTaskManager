using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryMgr.Models.Abstract
{
    public interface IQtyMeasurementsRepository
    {
        InventoryQtyMeasurements Get(int id);

        /// <summary>
        /// Get all InventoryQtyMeasurements that belong to a specific person, or are global measurements
        /// </summary>
        /// <param name="userId">int</param>        
        IEnumerable<InventoryQtyMeasurements> GetAll(int userId);

        InventoryQtyMeasurements Update(InventoryQtyMeasurements qtyMeasurement);
        InventoryQtyMeasurements Add(InventoryQtyMeasurements qtyMeasurement);
        void Delete(int id);
        InventoryQtyMeasurements GetByName(String qtyMeasurementName);
    }
}
