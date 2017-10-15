using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkloadProject.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public Teacher Teacher { get; set; }
        public Subject Subject { get; set; }
        public TypeLesson TypeLesson { get; set; }
        public int HourLoad { get; set; }
        public int HoursWorked { get; set; }
    }
}
