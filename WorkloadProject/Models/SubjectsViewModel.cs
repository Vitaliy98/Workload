using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkloadProject.Models
{
    public class SubjectsViewModel
    {
        public IEnumerable<Subject> Subjects { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterSubjectsViewModel FilterSubjectsViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}
