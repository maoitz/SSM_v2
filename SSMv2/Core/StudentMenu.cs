using Microsoft.EntityFrameworkCore;
using SSMv2.Models;

namespace SSMv2;

public class StudentDetailsResult
{
    public int StudentID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string SocialSecurityNumber { get; set; }
    public string ClassName { get; set; }
    public string? Courses { get; set; }
}

public class StudentMenu
{
    public static void ShowMenu()
    {
        const bool isRunning = true;
        while (isRunning)
        {
            var choice = Menu.ShowMenu("| Student Menu |", Menu.GetStudentMenuOptions());
            switch (choice)
            {
                case Menu.Options.ViewAllStudents:
                    ViewAllStudents();
                    break;
                case Menu.Options.AddNewStudent:
                    AddNewStudent();
                    break;
                case Menu.Options.SearchStudentById:
                    SearchStudentById();
                    break;
                case Menu.Options.EnrollStudent:
                    EnrollStudent();
                    break;
                case Menu.Options.SetStudentGrade:
                    GradeStudent();
                    break;
                case Menu.Options.Back:
                    return;
            }
        }
    }

    // View all students
    private static void ViewAllStudents()
    {
        Console.Clear();
        using var db = new AppDbContext();
        var students = db.Students
            .Include(c => c.ClassNavigation)
            .ToList();

        Console.WriteLine("| All Students |");
        Console.WriteLine("ID".PadRight(5) +
                          "Name".PadRight(25) +
                          "Class".PadRight(10));


        foreach (var s in students)
        {
            Console.WriteLine($"{s.StudentID.ToString().PadRight(5)}" +
                              $"{(s.FirstName + " " + s.LastName).PadRight(25)}" +
                              $"{s.ClassNavigation.ClassName.PadRight(10)}");
        }

        Console.WriteLine("-----------------------------");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }


    // Add new student
    private static void AddNewStudent()
    {
        Console.Clear();
        Console.WriteLine("| Add New Student |");
        using var db = new AppDbContext();
        Console.WriteLine("Enter student first name:");
        var firstName = Console.ReadLine();
        Console.WriteLine("Enter student last name:");
        var lastName = Console.ReadLine();

        var isDone = false;
        while (!isDone)
        {
            Console.WriteLine("[1] 9A");
            Console.WriteLine("[2] 9B");
            Console.WriteLine("[3] 9C");
            Console.WriteLine("Enter student class:");
            if (int.TryParse(Console.ReadLine(), out var classId))
            {
                Console.WriteLine("Enter Social Security Number [YYYYMMDD-XXXX]:");
                var ssn = Console.ReadLine();

                if (ssn.Length != 13)
                {
                    Console.WriteLine("Invalid SSN. Please enter a valid SSN.");
                    Thread.Sleep(1000);
                    return;
                }

                var student = new Models.Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Class = classId,
                    SocialSecurityNumber = ssn
                };

                db.Students.Add(student);
                db.SaveChanges();

                Console.WriteLine("Student added successfully!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                isDone = true;
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
        }
    }

    // Search student by ID
    private static void SearchStudentById()
    {
        Console.Clear();
        using var db = new AppDbContext();
        Console.WriteLine("| Search Student By ID |\n");
        Console.WriteLine("Current Students:");
        var students = db.Students
            .ToList();

        Console.WriteLine("ID".PadRight(5) + "Name".PadRight(25));
        foreach (var s in students)
        {
            Console.WriteLine($"{s.StudentID.ToString().PadRight(5)}" +
                              $"{(s.FirstName + " " + s.LastName).PadRight(25)}");
        }

        Console.WriteLine("-----------------------------");
        Console.Write("Enter student ID: ");
        if (int.TryParse(Console.ReadLine(), out var studentId))
        {
            var results = db.StudentDetailsResults
                .FromSql($"EXEC GetStudentDetails @StudentID = {studentId}")
                .AsEnumerable()
                .FirstOrDefault();

            if (results != null)
            {
                Console.Clear();
                Console.WriteLine("== Student Details ==");
                Console.WriteLine($"ID: {results.StudentID}");
                Console.WriteLine($"Name: {results.FirstName} {results.LastName}");
                Console.WriteLine($"Address: {results.Address}");
                Console.WriteLine($"Phone Number: {results.PhoneNumber}");
                Console.WriteLine($"Social Security Number: {results.SocialSecurityNumber}");
                Console.WriteLine($"Class: {results.ClassName}");
                Console.WriteLine($"Courses: {results.Courses}");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid student ID.");
        }

        Console.WriteLine("-----------------------------");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    // Enroll student in course
    private static void EnrollStudent()
    {
        using var db = new AppDbContext();

        Console.Clear();
        var enrollStudent = ChooseStudentId();
        Console.Clear();
        var chooseCourse = ChooseCourseId();

        // Check if student is already enrolled in chosen course
        if (db.StudentCourses.Any(sc => sc.StudentID == enrollStudent && sc.CourseID == chooseCourse))
        {
            Console.Clear();
            Console.WriteLine("\nStudent is already enrolled in this course.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        var studentCourse = new StudentCourse
        {
            StudentID = enrollStudent,
            CourseID = chooseCourse
        };

        db.StudentCourses.Add(studentCourse);
        db.SaveChanges();

        Console.WriteLine("\nStudent enrolled successfully!");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    // Choose valid student id
    private static int ChooseStudentId()
    {
        using var db = new AppDbContext();
        var students = db.Students
            .ToList();

        var isValid = false;
        while (!isValid)
        {
            Console.Clear();
            Console.WriteLine("| Students |");
            Console.WriteLine(" =Choose Student= ");
            Console.WriteLine("ID".PadRight(5) + "Name".PadRight(25));
            foreach (var s in students)
            {
                Console.WriteLine($"{s.StudentID.ToString().PadRight(5)}" +
                                  $"{(s.FirstName + " " + s.LastName).PadRight(25)}");
            }

            Console.WriteLine("-----------------------------");
            Console.Write("Enter student ID: ");
            var choice = Menu.GetMenuChoice(students.Count);
            if (students.Any(s => s.StudentID == choice))
            {
                return (int)choice;
            }
            else
            {
                Menu.InvalidOption();
            }
        }

        return 0;
    }


    // Choose valid course id
    private static int ChooseCourseId()
    {
        using var db = new AppDbContext();
        var courses = db.Courses
            .ToList();

        var isValid = false;
        while (!isValid)
        {
            Console.Clear();
            Console.WriteLine("| Courses |");
            Console.WriteLine("Current Courses:");
            Console.WriteLine("ID".PadRight(5) + "Name".PadRight(25));
            foreach (var c in courses)
            {
                Console.WriteLine($"{c.CourseID.ToString().PadRight(5)}" +
                                  $"{c.CourseName.PadRight(25)}");
            }

            Console.WriteLine("-----------------------------");
            Console.Write("Enter course ID: ");
            var choice = Menu.GetMenuChoice(courses.Count);
            if (courses.Any(c => c.CourseID == choice))
            {
                return (int)choice;
            }
            else
            {
                Menu.InvalidOption();
            }
        }

        return 0;
    }

    // Choose valid employee id
    private static int ChooseEmployeeId()
    {
        using var db = new AppDbContext();
        var employees = db.Employees
            .ToList();

        var isValid = false;
        while (!isValid)
        {
            Console.WriteLine("| Employees |");
            Console.WriteLine("Current Employees:");
            Console.WriteLine("ID".PadRight(5) + "Name".PadRight(25));
            foreach (var e in employees)
            {
                Console.WriteLine($"{e.EmployeeID.ToString().PadRight(5)}" +
                                  $"{(e.FirstName + " " + e.LastName).PadRight(25)}");
            }

            Console.WriteLine("-----------------------------");
            Console.Write("Enter employee ID: ");
            var choice = Menu.GetMenuChoice(employees.Count);
            if (employees.Any(e => e.EmployeeID == choice))
            {
                return (int)choice;
            }
            else
            {
                Menu.InvalidOption();
            }
        }

        return 0;
    }

    // Choose valid grade value
    private static decimal ChooseGradeValue()
    {
        var isValid = false;
        while (!isValid)
        {
            Console.Clear();
            Console.WriteLine("| Grade Value |");
            Console.WriteLine("Enter grade value [0.0 - 5.0]:");
            var choice = Console.ReadLine();
            if (decimal.TryParse(choice, out var gradeValue) && gradeValue >= 0 && gradeValue <= 5)
            {
                return gradeValue;
            }
            else
            {
                Menu.InvalidOption();
            }
        }

        return 0;
    }

    // Grade student
    private static void GradeStudent()
    {
        Console.Clear();

        var employeeId = ChooseTeacher();
        var studentId = ChooseStudentId();
        var courseId = ChooseEnrolledCourse(studentId);
        var gradeValue = ChooseGradeValue();

        // Method to add a grade to a student for a specific course
        SetStudentGrade(gradeValue, employeeId, studentId, courseId);

    }

    // Set student grade
    private static void SetStudentGrade(decimal gradeValue, int employeeIdInput, int courseIdInput, int studentIdInput)
    {
        using (var context = new AppDbContext())
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // Get teacher
                    var teacher = context.Employees
                        .FirstOrDefault(e => e.EmployeeID == employeeIdInput);
                    if (teacher == null)
                    {
                        throw new Exception("Teacher not found.");
                    }

                    // Control if course is active
                    var course = context.Courses
                        .FirstOrDefault(c => c.IsActive && c.CourseID == courseIdInput);
                    if (course == null)
                    {
                        throw new Exception("No active courses found.");
                    }

                    // Control if student is enrolled in course
                    var enrollmentExists = context.StudentCourses
                        .FirstOrDefault(sc => sc.StudentID == studentIdInput && sc.CourseID == courseIdInput);

                    if (enrollmentExists == null)
                    {
                        throw new Exception("Student is not enrolled in this course.");
                    }

                    // Check if grade already exists
                    var existingGrade = context.Grades
                        .FirstOrDefault(g => g.StudentID == studentIdInput && g.CourseID == courseIdInput);

                    while (true)
                    {
                        if (existingGrade != null)
                        {
                            // Update current grade
                            Console.WriteLine("Grade already exists. Do you want to update the grade? [Y/N]");
                            var updateGrade = Console.ReadLine();

                            var gradeUpdated = false;
                            while (!gradeUpdated)
                            {
                                if (updateGrade?.ToLower() == "y")
                                {
                                    existingGrade.Grade1 = gradeValue;
                                    existingGrade.DateGraded = DateOnly.FromDateTime(DateTime.Now);
                                    context.Grades.Update(existingGrade);
                                    Console.WriteLine("Grade updated successfully!");
                                    Thread.Sleep(1000);
                                    gradeUpdated = true;
                                }
                                else if (updateGrade?.ToLower() == "n")
                                {
                                    Console.WriteLine("Grade not updated.");
                                    Thread.Sleep(1000);
                                }
                                else
                                {
                                    Menu.InvalidOption();
                                }
                            }
                        }
                        else
                        {
                            // Create new grade
                            var grade = new Grade()
                            {
                                Grade1 = gradeValue,
                                DateGraded = DateOnly.FromDateTime(DateTime.Now),
                                EmployeeID = teacher.EmployeeID,
                                StudentCourseID = enrollmentExists.StudentCourseID,
                                CourseID = course.CourseID,
                                StudentID = enrollmentExists.StudentID
                            };
                            context.Grades.Add(grade);
                        }

                        // Save changes
                        context.SaveChanges();

                        // Confirm transaction and break loop
                        transaction.Commit();
                        
                        Console.WriteLine("Grade added successfully!");
                        Thread.Sleep(1000);
                        break;
                    }
                }
                catch (Exception e)
                {
                    // Rollback in case of error
                    transaction.Rollback();
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();

                }
            }
        }
    }

    // Choose valid teacher
    private static int ChooseTeacher()
    {
        var endLoop = false;
        while (!endLoop)
        {
            using var db = new AppDbContext();

            // map all position=teachers from the database
            var teachers = db.Employees
                .Where(e => e.Position == "Teacher")
                .ToList();

            if (!teachers.Any())
            {
                Console.WriteLine("No teachers available.");
                return 0;
            }

            // List all teachers
            Console.WriteLine("| Grade Student |");
            Console.WriteLine(" =Choose Teacher= ");
            Console.WriteLine("ID".PadRight(10) + "Name".PadRight(25));
            foreach (var t in teachers)
            {
                Console.WriteLine($"{t.EmployeeID.ToString().PadRight(10)}" +
                                  $"{(t.FirstName + " " + t.LastName).PadRight(25)}");
            }

            Console.WriteLine("-----------------------------");
            Console.Write("Enter Teacher ID: ");

            Console.WriteLine();
            Console.Write("[ ]");
            Console.CursorVisible = true;
            Console.SetCursorPosition(1, Console.CursorTop);
            var keyInfo = Console.ReadKey(true);
            Console.SetCursorPosition(0, Console.CursorTop);

            // Input validation
            if (int.TryParse(keyInfo.KeyChar.ToString(), out var choice))
            {
                // Check if the choice is a valid teacher ID
                if (teachers.Any(t => t.EmployeeID == choice))
                {
                    return choice; // Return the valid teacher ID
                }
            }

            // Should the input be invalid, the user will be prompted to try again
            Menu.InvalidOption();
            Console.Clear();
        }

        return 0;
    }

    // Choose valid course
    private static int ChooseEnrolledCourse(int studentId)
    {
        var endLoop = false;
        while (!endLoop)
        {
            using var db = new AppDbContext();
            var enrolledCourses = db.StudentCourses
                .Where(sc => sc.StudentID == studentId)
                .Include(sc => sc.Course)
                .Select(sc => sc.CourseID)
                .ToList();

            if (!enrolledCourses.Any())
            {
                Console.WriteLine("No courses found for this student.");
                Thread.Sleep(1000);
                return 0;
            }

            // List all courses
            Console.Clear();
            Console.WriteLine("| Grade Student |");
            Console.WriteLine(" =Choose Course=");
            Console.WriteLine("Current Available Courses:");
            Console.WriteLine("ID".PadRight(5) + "Name".PadRight(25));
            foreach (var course in enrolledCourses)
            {
                Console.WriteLine($"{course.ToString().PadRight(5)}" +
                                  $"{db.Courses.Find(course).CourseName.PadRight(25)}");
            }

            Console.WriteLine("-----------------------------");
            Console.Write("Enter course ID: ");
            Console.WriteLine();
            Console.Write("[ ]");
            Console.CursorVisible = true;
            Console.SetCursorPosition(1, Console.CursorTop);
            var keyInfo = Console.ReadKey(true);
            Console.SetCursorPosition(0, Console.CursorTop);

            // Input validation
            if (int.TryParse(keyInfo.KeyChar.ToString(), out var choice))
            {
                // Check if the choice is a valid course ID
                if (enrolledCourses.Any(c => c == choice))
                {
                    return choice; // Return the valid course ID
                }
            }

            // Should the input be invalid, the user will be prompted to try again
            Menu.InvalidOption();
        }

        return 0;
    }
}
