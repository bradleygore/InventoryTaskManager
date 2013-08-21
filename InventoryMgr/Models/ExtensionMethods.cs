using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryMgr.Models
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts to a select list, and sets the selected item to the ID of the current category.
        /// </summary>
        /// <param name="cats"></param>
        /// <param name="selectedId">int</param>
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<InventoryCategory> cats, int selectedId)
        {
            int iLengthToTake = 20;

            return cats.OrderBy(c => c.CategoryName)
                .Select(c => new SelectListItem
                {
                    Selected = (c.Id == selectedId),
                    Text = c.CategoryName + (c.CategoryDescription == null ? "" : " - " + (c.CategoryDescription.Length > iLengthToTake ?
                                                        c.CategoryDescription.Substring(0, iLengthToTake) + "..." :
                                                        c.CategoryDescription)),
                    Value = c.Id.ToString()
                });
        }

        /// <summary>
        /// Converts to a select list, not setting anything as the selected item.
        /// </summary>
        /// <param name="cats"></param>
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<InventoryCategory> cats)
        {
            int iLengthToTake = 20;

            return cats.OrderBy(c => c.CategoryName)
                .Select(c => new SelectListItem
                {
                    Text = c.CategoryName + (c.CategoryDescription == null ? "" : " - " + (c.CategoryDescription.Length > iLengthToTake ?
                                                        c.CategoryDescription.Substring(0, iLengthToTake) + "..." :
                                                        c.CategoryDescription)),
                    Value = c.Id.ToString()
                });
        }

        /// <summary>
        /// Converts to a select list, and sets the selected item to the ID of the current Quantity Measurement.
        /// </summary>
        /// <param name="qtys"></param>
        /// <param name="selectedId">int</param>
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<InventoryQtyMeasurements> qtys, int selectedId)
        {
            return qtys.OrderBy(q => q.UnitOfMeasure)
                .Select(q => new SelectListItem
                {
                    Selected = (q.Id == selectedId),
                    Text = q.UnitOfMeasure,
                    Value = q.Id.ToString()
                });
        }

        /// <summary>
        /// Converts to a select list, not setting anything as the selected item.
        /// </summary>
        /// <param name="qtys"></param>
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<InventoryQtyMeasurements> qtys)
        {
            return qtys.OrderBy(q => q.UnitOfMeasure)
                .Select(q => new SelectListItem
                {
                    Text = q.UnitOfMeasure,
                    Value = q.Id.ToString()
                });
        }

        /// <summary>
        /// Converts InventoryItems to a select list, setting the selected item based on the passed in InventoryItem Id.
        /// </summary>
        /// <param name="invItems">IEnumerable(InventoryItem)</param>
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<InventoryItem> invItems, int selectedId)
        {

            return invItems.OrderBy(i => i.ItemName)
                .Select(i => new SelectListItem
                {
                    Selected = (i.Id == selectedId),
                    Text = i.ItemName + (i.ItemDescription == null ? "" : " - " + (i.ItemDescription.Length > 20 ? i.ItemDescription.Substring(0, 20) + "..." : i.ItemDescription)),
                    Value = i.Id.ToString()
                });
        }

        /// <summary>
        /// Converts InventoryItems to a select list, not setting anything as the selected item.
        /// </summary>
        /// <param name="invItems">IEnumerable(InventoryItem)</param>
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<InventoryItem> invItems)
        {

            return invItems.OrderBy(i => i.ItemName)
                .Select(i => new SelectListItem
                {
                    Text = i.ItemName + (i.ItemDescription == null ? "" : " - " + (i.ItemDescription.Length > 20 ? i.ItemDescription.Substring(0, 20) + "..." : i.ItemDescription)),
                    Value = i.Id.ToString()
                });
        }

        /// <summary>
        /// Returns an IEnumerable of TaskList as an IEnumerable of SelectListItem with the appropriate one selected to be used in an HTML.DropDownListFor(...)
        /// </summary>
        /// <param name="tasks">IEnumerable of TaskList</param>
        /// <param name="selectedId">ID to mark as selected</param>
        /// <returns>IEnumerable of SelectListItem</returns>
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<TaskList> tasks, int selectedId)
        {
            return tasks.OrderBy(t => t.TaskName)
                .Select(t => new SelectListItem
                {
                    Selected = (t.Id == selectedId),
                    Text = t.TaskName,
                    Value = t.Id.ToString()
                });
        }

        /// <summary>
        /// Returns an IEnumerable of TaskList as an IEnumerable of SelectListItem with none selected to be used in an HTML.DropDownListFor(...)
        /// </summary>
        /// <param name="tasks">IEnumerable of TaskList</param>
        /// <returns>IEnumerable of SelectListItem</returns>
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<TaskList> tasks)
        {
            return tasks.OrderBy(t => t.TaskName)
                .Select(t => new SelectListItem
                {
                    Text = t.TaskName,
                    Value = t.Id.ToString()
                });
        }
    }
}