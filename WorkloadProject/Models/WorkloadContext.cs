using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WorkloadProject.Models
{
    public class WorkloadContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<TypeLesson> TypeLessons { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TimeLesson> TimeLessons { get; set; }
        public DbSet<Journal> Journals { get; set; }

        public WorkloadContext(DbContextOptions<WorkloadContext> options)
            :base(options)
        {

        }
    }
}
