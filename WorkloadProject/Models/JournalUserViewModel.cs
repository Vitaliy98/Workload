using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkloadProject.Models
{
    public class JournalUserViewModel
    {
        public IEnumerable<Journal> Journals { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterSubjectsViewModel FilterSubjectsViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}
