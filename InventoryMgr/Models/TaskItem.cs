using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace InventoryMgr.Models
{
    public class TaskItem
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Completed { get; set; }

        [Display(Name="Created Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime TaskDate { get; set; }

        [Display(Name = "Completed Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? CompletedDate { get; set; }
        
        
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "*")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Task List")]
        public int TaskListId { get; set; }
    }
}