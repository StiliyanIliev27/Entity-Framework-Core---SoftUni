using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public Student()
        {
            Courses = new List<Course>();
            StudentsCourses = new List<StudentCourse>();
            Homeworks = new List<Homework>();
        }

        [Key]
        public int StudentId { get; set; }

        [MaxLength(Constants.Constants.MaxStudentNameLength)]
        [Unicode(true)]
        [Required]
        public string Name { get; set; }

        [MaxLength(Constants.Constants.MaxStudentPhoneNumberLength)]
        [Unicode(false)]
        public string? PhoneNumber { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }
        public DateTime? Birthday { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<Homework> Homeworks { get; set; }
        public ICollection<StudentCourse> StudentsCourses { get; set; }
    }
}
