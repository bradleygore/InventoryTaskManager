 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace InventoryMgr.Models
{
    public class InventoryContextInitializer : DropCreateDatabaseIfModelChanges<InventoryContext>
    {
        protected override void Seed(InventoryContext context)
        {
            //SEED CATEGORIES
            var categoryId = context.Categories.Add(new InventoryCategory
            {
                CategoryName = "Food",
                CategoryDescription = "General food items to keep track of in inventory",
                UserId = 1,
                IsGlobal = true
            });

            context.Categories.Add(new InventoryCategory
            {
                CategoryName = "Cleaning Supplies",
                CategoryDescription = "General cleaning items to keep track of in inventory",
                UserId = 1,
                IsGlobal = true
            });

            context.Categories.Add(new InventoryCategory
            {
                CategoryName = "Clothing",
                CategoryDescription = "General clothing items to keep track of in inventory",
                UserId = 1,
                IsGlobal = true
            });

            context.Categories.Add(new InventoryCategory
            {
                CategoryName = "Gifts",
                CategoryDescription = "General listing of gifts to use for special occasions",
                UserId = 1,
                IsGlobal = true
            });
            context.SaveChanges();//Save the newly created Categories

            //SEED QtyMeasurements
            context.QtyMeasurements.Add(new InventoryQtyMeasurements
            {
                UnitOfMeasure = "Pair",
                UserId = 1,
                IsGlobal = true
            });
            context.QtyMeasurements.Add(new InventoryQtyMeasurements
            {
                UnitOfMeasure = "Gallons",
                UserId = 1,
                IsGlobal = true
            });
            context.QtyMeasurements.Add(new InventoryQtyMeasurements
            {
                UnitOfMeasure = "Dozen",
                UserId = 1,
                IsGlobal = true
            });
            context.QtyMeasurements.Add(new InventoryQtyMeasurements
            {
                UnitOfMeasure = "Cans",
                UserId = 1,
                IsGlobal = true
            });
            context.QtyMeasurements.Add(new InventoryQtyMeasurements
            {
                UnitOfMeasure = "Containers",
                UserId = 1,
                IsGlobal = true
            });
            context.QtyMeasurements.Add(new InventoryQtyMeasurements
            {
                UnitOfMeasure = "Loaves",
                UserId = 1,
                IsGlobal = true
            });
            context.SaveChanges();

            //SEED ITEMS
            context.Items.Add(new InventoryItem
            {
                InventoryCategory = context.Categories.Single(c => c.CategoryName == "Food"),
                InventoryCategoryId = context.Categories.Single(c => c.CategoryName == "Food").Id,
                ItemDescription = "Large Carton - From Publix",
                ItemLastUpdated = DateTime.Now,
                ItemName = "Eggs",
                ItemQuantity = 0.5,
                InventoryQtyMeasurements = context.QtyMeasurements.Single(qm => qm.UnitOfMeasure == "Dozen"),
                InventoryQtyMeasurementsId = context.QtyMeasurements.Single(qm => qm.UnitOfMeasure == "Dozen").Id,
                UserId = 1
            });
            context.Items.Add(new InventoryItem
            {
                InventoryCategory = context.Categories.Single(c => c.CategoryName == "Food"),
                InventoryCategoryId = context.Categories.Single(c => c.CategoryName == "Food").Id,
                ItemDescription = "Italian Style - No Preservatives",
                ItemLastUpdated = DateTime.Now,
                ItemName = "Bread",
                ItemQuantity = 0.25,
                InventoryQtyMeasurements = context.QtyMeasurements.Single(qm => qm.UnitOfMeasure == "Loaves"),
                InventoryQtyMeasurementsId = context.QtyMeasurements.Single(qm => qm.UnitOfMeasure == "Loaves").Id,
                UserId = 1
            });
            context.Items.Add(new InventoryItem
            {
                InventoryCategory = context.Categories.Single(c => c.CategoryName == "Clothing"),
                InventoryCategoryId = context.Categories.Single(c => c.CategoryName == "Clothing").Id,
                ItemDescription = "White Ankle-High Cotton",
                ItemLastUpdated = DateTime.Now,
                ItemName = "Socks",
                ItemQuantity = 3,
                InventoryQtyMeasurements = context.QtyMeasurements.Single(qm => qm.UnitOfMeasure == "Pair"),
                InventoryQtyMeasurementsId = context.QtyMeasurements.Single(qm => qm.UnitOfMeasure == "Pair").Id,
                UserId = 1
            });
            context.SaveChanges();
        }
    }
}