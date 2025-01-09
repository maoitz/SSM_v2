using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SSMv2.Models;

namespace SSMv2
{
    public class CourseMenu
    {
        public static void ShowMenu()
        {
            const bool isRunning = true;
            while (isRunning)
            {
                var choice = Menu.ShowMenu("| Course Menu |", Menu.GetCourseMenuOptions());
                switch (choice)
                {
                    case Menu.Options.ViewAllActiveCourses:
                        ViewAllActiveCourses();
                        break;
                    case Menu.Options.ViewAllGrades:
                        ViewAllGrades();
                        break;
                    case Menu.Options.Back:
                        return;
                }
            }
        }

        // View all active courses
        private static void ViewAllActiveCourses()
        {
            Console.Clear();
            using var db = new AppDbContext();
            var activeCourses = db.Courses.Where(c => c.IsActive).ToList();

            if (activeCourses.Count == 0)
            {
                Console.WriteLine("No active courses found.");
                Thread.Sleep(1000);
                return;
            }

            Console.WriteLine("| Active Courses |");
            Console.WriteLine("ID".PadRight(5) + "Name".PadRight(25));
            foreach (var course in activeCourses)
            {
                Console.WriteLine($"{course.CourseID.ToString().PadRight(5)}{course.CourseName.PadRight(25)}");
            }

            Console.WriteLine("-----------------------------");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        // View all grades
        private static void ViewAllGrades()
        {
            Console.Clear();
            using var db = new AppDbContext();
            // Include the CourseNavigation, StudentNavigation, and EmployeeNavigation properties
            // to get the CourseName, StudentName, and EmployeeName instead of the IDs
            var grades = db.Grades
                .Include(g => g.Course)
                .Include(g => g.Student)
                .Include(g => g.Employee)
                .ToList();
            
            if (grades.Count == 0)
            {
                Console.WriteLine("No grades found.");
                Thread.Sleep(1000);
                return;
            }
            
            Console.WriteLine("| All Grades For Active Courses |");
            
            Console.WriteLine("Course".PadRight(15) +
                              "Student".PadRight(20) +
                              "Grade".PadRight(10) +
                              "Graded By".PadRight(20) +
                              "Date Graded".PadRight(20));
            
            foreach (var g in grades)
            {
                Console.WriteLine($"{g.Course.CourseName.PadRight(15)}" +
                                  $"{g.Student.FirstName + " " + g.Student.LastName.PadRight(15)}" +
                                  $"{g.Grade1.ToString().PadRight(10)}" +
                                  $"{g.Employee.FirstName + " " + g.Employee.LastName.PadRight(15)}" +
                                  $"{g.DateGraded.ToString().PadRight(20)}");
            }

            Console.WriteLine("-----------------------------");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}