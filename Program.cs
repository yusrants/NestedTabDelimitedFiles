using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolReportSystem
{


    class Program
    {
        static void Main(string[] args)
        {
            string Fpath = Path.Combine(Directory.GetCurrentDirectory(), @"/test.txt");
            //Giving the path as an argument
            CreateReport(Fpath);
           //TestAddedData();
            ShowReport();
        }


        public static List<Teacher> teachers = new List<Teacher>();
        public static List<Student> students = new List<Student>();
        public static List<Course> courses = new List<Course>();
        public static List<Exam> exams = new List<Exam>();


        public static void ShowReport()
        {
            Console.WriteLine("School Reporting System");
            Console.WriteLine("The results are shown as:");
            int count = 0;

           foreach(var exam in exams)
           {
               count++;
               Console.WriteLine("Result #" + count);
               Console.Write("Student " + exam.StudentName + " was taught the course " + courses[exam.course-1].Title + " by instructors " );
               for (int i=0; i< exam.teacher.Count; i++)
               {
                   if (exam.teacher.Count > 1)
                       Console.Write(teachers[exam.teacher[i] - 1].Name + " and ");
                   else
                       Console.Write(teachers[exam.teacher[i] - 1].Name );

               }
               Console.Write("\n");
               //For calculating percentage:
               decimal marks= Convert.ToDecimal(exam.marks);
               decimal Tmarks= Convert.ToDecimal(courses[exam.course - 1].TotalMarks);
               if ((marks/Tmarks)*100 >= 60)
               {
                   Console.Write("He passed the course by getting " + exam.marks + " out of " + courses[exam.course - 1].TotalMarks + "\n\n");
               }
               else
               {
                   Console.Write("He failed the course by getting " + exam.marks + " out of " + courses[exam.course - 1].TotalMarks + "\n\n");
               }
           }
           Console.ReadLine();
        }

        public static void TestAddedData()
        {
            //Testing the added Data:
            foreach (var tempr in teachers)
            {
                Console.WriteLine("Teacher#" + tempr.key);
                Console.WriteLine(tempr.StaffID);
                Console.WriteLine(tempr.Name);
                Console.WriteLine(tempr.Qualifications);
                Console.WriteLine("\n");

            }

            foreach (var tempr in courses)
            {
                Console.WriteLine("Course#" + tempr.key);
                Console.WriteLine(tempr.ID);
                Console.WriteLine(tempr.Title);
                Console.WriteLine(tempr.TotalMarks);
                Console.WriteLine("\n");


            }

            foreach (var tempr in students)
            {
                Console.WriteLine("Student#" + tempr.key);
                Console.WriteLine(tempr.ID);
                Console.WriteLine(tempr.Name);
                Console.WriteLine("\n");


            }

            foreach (var tempr in exams)
            {
               Console.WriteLine("Now printing exams:");
                Console.WriteLine(tempr.course);
                foreach (var teach in tempr.teacher)
                {
                    Console.WriteLine("The teachers are:" + teach);
                }
                Console.WriteLine(tempr.marks);
                
                Console.WriteLine("\n");
            }

            Console.ReadLine();
        }


        

        public static void CreateReport(string docPath)
        {
            
            //Initialzing the number of rows for each:
            int rows = 3;
            //Readin the txt file and converting storing the lines into a list
            List<string> lines = File.ReadAllLines(docPath).Skip(10).ToList();


            for (int i = 2; i < lines.Count; i++)
            {


                int primary = 0;
                while (lines[i].Contains("\t\t"))
                {
                    Teacher teach = new Teacher();
                   // Console.WriteLine("reached teachers");
                    //Extracting data from the file:
                    string StaID = lines[i].Substring(lines[i].IndexOf(':') + 2);
                    string nem = lines[i + (rows - 2)].Substring(lines[i + (rows - 2)].IndexOf(':') + 2);
                    string Qual = lines[i + (rows - 1)].Substring(lines[i + (rows - 1)].IndexOf(':') + 2);
                    primary++;
                    //Adding to the teachers list:
                    teach.key = primary;
                    teach.StaffID = StaID;
                    teach.Name = nem;
                    teach.Qualifications = Qual;
                    teachers.Add(teach);
                    //Moving to the next entry:
                    i = i + (rows + 1);

                    //If this statement is satisfied, time to move to next entity
                    if (!(lines[i].Contains("\t\t")))
                    {

                        //resetting primary key:
                        primary = 0;

                    }


                    if (lines[i - 1].Contains("Courses"))
                    {

                        i = i + 1;

                        while (lines[i].Contains("\t\t"))
                        {
                            Course cour = new Course();
                            //Console.WriteLine("reached courses");
                            //Extracting data from the file:
                            string ident = lines[i].Substring(lines[i].IndexOf(':') + 2);
                            string title = lines[i + (rows - 2)].Substring(lines[i + (rows - 2)].IndexOf(':') + 2);
                            string str_marks = lines[i + (rows - 1)].Substring(lines[i + (rows - 1)].IndexOf(':') + 2);
                            // Converting marks into int for later use in calculations:
                            int marks = Convert.ToInt32(str_marks);
                            primary++;
                            //Adding to the teachers list:
                            cour.key = primary;
                            cour.ID = ident;
                            cour.Title = title;
                            cour.TotalMarks = marks;
                            courses.Add(cour);
                           
                            i = i + (rows + 1);
                            if ((lines[i].Contains("\t\t")))
                            {
                                continue;

                            }
                            //Moving to the next entry:
                            else if (lines[i - 1].Contains("Students"))
                            {
                               
                                i = i + 1;
                                primary = 0;
                             while (lines[i].Contains("\t\t"))
                                {

                                    Student std = new Student();
                                   // Console.WriteLine("reached students");

                                    //Storing the number of exams for each student:
                                    
                                    //Extracting data from the file:
                                    string stdid = lines[i].Substring(lines[i].IndexOf(':') + 2);
                                    string name = lines[i + (rows - 2)].Substring(lines[i + (rows - 2)].IndexOf(':') + 2);
                                    primary++;
                                    //Adding data:
                                    std.key = primary;
                                    std.Name = name;
                                    std.ID = stdid;
                                    students.Add(std);
                                    //Moving to the exam results part of the particular student.
                                    i = i + (rows + 1);
                                    int no_exams = 0;

                                    while (lines[i].Contains("\t\t\t\t"))
                                    {
                                        Exam exam1 = new Exam(std);

                                       // Console.WriteLine("result added");
                                        
                                        no_exams++;

                                        //Extracting data from exams:

                                        string course_str = lines[i].Substring(lines[i].LastIndexOf('/') + 1);
                                        string marks_str = lines[i + (rows - 1)].Substring(lines[i + (rows - 1)].IndexOf(":") + 2);
                                        List<string> ins_str = new List<string>();
                                        if (lines[i + (rows - 2)].Contains(","))
                                        {
                                            List<string> temp = lines[i + (rows - 2)].Split(',').ToList();
                                            foreach (var str in temp)
                                            {
                                                ins_str.Add(str);
                                            }
                                        }
                                        else
                                            ins_str.Add(lines[i + (rows - 2)]);
                                        int course_id = Convert.ToInt32(course_str);
                                        int course_marks = Convert.ToInt32(marks_str);

                                        List<int> instructors = new List<int>();
                                        foreach (var str in ins_str)
                                        {


                                            for (int k = 0; k < str.Length; k++)
                                            {
                                                int current;
                                                if (Char.IsDigit(str[k]))
                                                {

                                                    current = Convert.ToInt32(str[k]);
                                                    instructors.Add(current-48);


                                                }

                                            }

                                        }
                                        //Adding to exam class
                                        exam1.marks = course_marks;
                                        exam1.teacher = new List<int>();
                                        foreach (var instruct in instructors)
                                        {

                                            exam1.teacher.Add(instruct);
                                        }
                                        exam1.key = no_exams;
                                        exam1.course = course_id;
                                        exams.Add(exam1);
                                        //Moving to the next entry:
                                        if (i < 52)
                                        {
                                            i = i + (rows + 1);
                                            no_exams = 0;
                                            if (!(lines[i].Contains("\t\t\t\t")))
                                            {
                                                break;
                                            }
                                            else
                                                continue;
                                        }
                                        else
                                            return;
                                        
                                    }
                                    
                                    
                                }
                     
                            }

                        }
                    }

                }
            }


                               

           


        }
    }
}