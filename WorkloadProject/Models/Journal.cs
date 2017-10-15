using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkloadProject.Models
{
    public class Journal
    {
        public int Id { get; set; }
        public Lesson Lesson { get; set; }
        public DateTime Date { get; set; }
        public TimeLesson TimeLesson { get; set; }
        public string Theme { get; set; }
    }
}
