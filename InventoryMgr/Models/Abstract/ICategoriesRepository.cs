using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InventoryMgr.Models.Abstract
{
    public interface ICategoriesRepository
    {
        InventoryCategory Get(int id);

        /// <summary>
        /// Gets all categories that are either Global or belong to a specific user
        /// </summary>
        /// <returns></returns>
        IEnumerable<InventoryCategory> GetAll(int userId);

        InventoryCategory Add(InventoryCategory category);
        InventoryCategory Update(InventoryCategory category);
        void Delete(int categoryId);

        /// <summary>
        /// Use this to pass in the current user's Id and the name of one they want to create.
        /// If this returns something, then they already have this category and cannot create it again.
        /// </summary>
        /// <param name="name">string</param>
        /// <param name="userId">int</param>
        /// <returns>InventoryCategory</returns>
        InventoryCategory GetByNameAndUser(String category, int userId);
    }
}
