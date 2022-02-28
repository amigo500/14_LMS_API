using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Dto;

    public class CourseDto
    {
        public virtual string? Title { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate => StartDate.AddMonths(3); // +3 months after start
   }

