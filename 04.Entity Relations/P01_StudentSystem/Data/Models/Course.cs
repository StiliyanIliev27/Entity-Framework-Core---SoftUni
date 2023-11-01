using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        public Course()
        {
            Resources = new List<Resource>();
            Students = new List<Student>();
            StudentsCourses = new List<StudentCourse>();
            Homeworks = new List<Homework>();
        }

        [Key]
        public int CourseId { get; set; }

        [MaxLength(Constants.Constants.MaxCourseNameLength)]
        [Unicode]
        [Required]
        public string Name { get; set; }

        [Unicode]
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal Price { get; set; }
        public ICollection<Resource> Resources { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Homework> Homeworks { get; set; }
        public ICollection<StudentCourse> StudentsCourses { get; set; }
    }
}
