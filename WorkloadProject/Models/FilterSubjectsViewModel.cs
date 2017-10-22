using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkloadProject.Models
{
    public class FilterSubjectsViewModel
    {
        public FilterSubjectsViewModel(string subjectName)
        {
            SelectedSubjectName = subjectName;
        }

        public string SelectedSubjectName { get; private set; }
    }
}
