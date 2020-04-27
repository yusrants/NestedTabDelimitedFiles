using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolReportSystem
{
    public class Teacher
    {
        public int key { get; set; }
        public string StaffID { get; set; }
        public string Name { get; set; }
        public string Qualifications { get; set; }
    }

    public class Course
    {
        public int key { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
        public int TotalMarks { get; set; }


    }
    public class Student
    {
        public int key { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }


    }
   public class Exam
    {
        public Exam(Student std)
        {
           
           StudentName = std.Name;
           studentID = std.ID;
            

        }
        public string StudentName;
        public string studentID;
        public int key { get; set; }
        public int course { get; set; }
        public int marks { get; set; }
        public List<int> teacher { get; set; }
       
    }
}
