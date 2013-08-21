using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace InventoryMgr.Models
{
    public class InventoryQtyMeasurements
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Unit of Measure")]
        public String UnitOfMeasure { get; set; }

        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Property that is seen by all users, in addition to the ones they create that are only available to them.
        /// </summary>
        public bool? IsGlobal { get; set; }
    }
}