using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Title { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        public ICollection<Module>? Modules { get; set; }
    }
}
