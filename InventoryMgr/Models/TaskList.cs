using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace InventoryMgr.Models
{
    public class TaskList
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Task Name")]
        public String TaskName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Complete")]
        public bool IsComplete { get; set; }
        
        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "*")]
        public int UserId { get; set; }

        public virtual ICollection<TaskItem> Tasks { get; set; }
    }
}