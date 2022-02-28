using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Dto
{
    public class CourseUpdateDto : CourseDto
    {
        public override string? Title { get; set; }
        public override DateTime StartDate { get; set; }
        public override DateTime EndDate => StartDate.AddMonths(3); // 3 months after startdate
        public ICollection<Module>? Modules { get; set; }
    }
}
