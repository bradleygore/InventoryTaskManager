using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace InventoryMgr.Models
{
    public class InventoryCategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(100)]
        [Display(Name="Category Name")]
        public String CategoryName { get; set; }

        [StringLength(250)]
        [Display(Name = "Category Description")]
        public String CategoryDescription { get; set; }

        [Required(ErrorMessage = "*")]
        public int UserId { get; set; }

        /// <summary>
        /// Property that is seen by all users, in addition to the ones they create that are only available to them.
        /// </summary>
        public bool? IsGlobal { get; set; }

    }
}