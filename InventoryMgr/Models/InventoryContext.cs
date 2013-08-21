using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace InventoryMgr.Models
{
    public class InventoryContext : DbContext
    {
        public DbSet<InventoryCategory> Categories { get; set; }
        public DbSet<InventoryItem> Items { get; set; }
        public DbSet<InventoryQtyMeasurements> QtyMeasurements { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<InventoryTaskItem> InventoryTaskItems { get; set; }
        public DbSet<TaskList> TaskLists { get; set; }

        public InventoryContext() : base("name=EntitiesConnection")
        {
            Configuration.ProxyCreationEnabled = false;
        }
    }
}