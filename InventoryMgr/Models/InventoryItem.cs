using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryMgr.Models;
using System.ComponentModel.DataAnnotations;

namespace InventoryMgr.Models
{
    public class InventoryItem
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        
        [Required(ErrorMessage="*")]
        [StringLength(100)]
        [Display(Name="Item Name")]
        public String ItemName { get; set; }

        [StringLength(250)]
        [Display(Name = "Item Description")]
        public String ItemDescription { get; set; }

        [Display(Name = "Item Quantity")]
        [Required(ErrorMessage = "*")]
        public double? ItemQuantity { get; set; }

        [Display(Name = "Item Last Updated")]
        [DisplayFormat(DataFormatString="{0:d}", ApplyFormatInEditMode=true)]
        [DataType(DataType.Date)]
        public DateTime ItemLastUpdated { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Measurement")]
        public int InventoryQtyMeasurementsId { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Category")]
        public int InventoryCategoryId { get; set; }

        [Required(ErrorMessage = "*")]
        public int UserId { get; set; }

        public virtual InventoryQtyMeasurements InventoryQtyMeasurements { get; set; }
        public virtual InventoryCategory InventoryCategory { get; set; }


    }
}