using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkloadProject.Models
{
    public class FilterViewModel
    {
        public FilterViewModel(string surname)
        {
            SelectedSurname = surname;
        }

        public string SelectedSurname { get; private set; }  
    }
}
