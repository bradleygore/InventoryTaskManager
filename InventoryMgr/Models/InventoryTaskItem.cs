using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace InventoryMgr.Models
{
    public class InventoryTaskItem : TaskItem
    {
        [Display(Name="Inventory Item")]
        [Required(ErrorMessage = "*")]
        public int InventoryItemId { get; set; }
    }
}