using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace InventoryMgr.Models.ViewModels
{
    public class InventoryItemEditModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(100)]
        [Display(Name = "Item Name")]
        public String ItemName { get; set; }

        [StringLength(250)]
        [Display(Name = "Item Description")]
        public String ItemDescription { get; set; }

        [Display(Name = "Item Quantity")]
        public double? ItemQuantity { get; set; }

        [Display(Name = "Item Last Updated")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime ItemLastUpdated { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Measurement")]
        public int InventoryQtyMeasurementsId { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Category")]
        public int InventoryCategoryId { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Measurement")]
        public IEnumerable<SelectListItem> MeasurementsList { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Category")]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}