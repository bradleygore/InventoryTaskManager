using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Linq;
using InventoryMgr.Models.Abstract;

namespace InventoryMgr.Models.Repos
{
    public class QtyMeasurementsRepository : IQtyMeasurementsRepository
    {
        private InventoryContext _db { get; set; }

        public QtyMeasurementsRepository()
            : this(new InventoryContext())
        {

        }

        public QtyMeasurementsRepository(InventoryContext db)
        {
            _db = db;
        }

        public InventoryQtyMeasurements Get(int id)
        {
            return _db.QtyMeasurements.SingleOrDefault(m => m.Id == id);
        }

        public IEnumerable<InventoryQtyMeasurements> GetAll(int userId)
        {
            return _db.QtyMeasurements.Where(q => q.UserId == userId || q.IsGlobal == true);
        }

        public InventoryQtyMeasurements Add(InventoryQtyMeasurements qtyMeasurement)
        {
            _db.QtyMeasurements.Add(qtyMeasurement);
            _db.SaveChanges();
            return qtyMeasurement;
        }

        public InventoryQtyMeasurements Update(InventoryQtyMeasurements qtyMeasurement)
        {
            _db.Entry(qtyMeasurement).State = EntityState.Modified;
            _db.SaveChanges();
            return qtyMeasurement;
        }

        public void Delete(int qtyId)
        {
            var qtyMeasurement = _db.QtyMeasurements.Single(qm => qm.Id == qtyId);
            _db.QtyMeasurements.Remove(qtyMeasurement);
            _db.SaveChanges();
        }

        public InventoryQtyMeasurements GetByName(String qtyMeasurementName)
        {
            return _db.QtyMeasurements.SingleOrDefault(qm => qm.UnitOfMeasure == qtyMeasurementName);
        }

    }
}