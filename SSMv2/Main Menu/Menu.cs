namespace SSMv2;

public class Menu
{
    // Enum for all menu options (Adjustable)
    public enum Options
    {
        // Main Menu
        AdminMenu = 1, // Start at 1
        EmployeeMenu,
        StudentMenu,
        CourseMenu,
        
        // Admin Menu
        TotalSalaryPaidByDepartment,
        MedianSalaryByDepartment,

        // Employee Menu
        ViewAllEmployees,
        ViewEmployeesByDepartment,
        AddNewEmployee,
        
        // Student Menu
        ViewAllStudents,
        AddNewStudent,
        SearchStudentById,
        EnrollStudent,
        SetStudentGrade,

        // Course Menu
        ViewAllActiveCourses,
        ViewAllGrades,

        // Other
        Exit,
        Back
    }

    // Dictionary linking each option to a specific text
    private static readonly Dictionary<Options, string> Texts = new Dictionary<Options, string>
    {
        // Main Menu
        { Options.AdminMenu, "Admin Menu" },
        { Options.EmployeeMenu, "Manage Employees" },
        { Options.StudentMenu, "Manage Students" },
        { Options.CourseMenu, "Manage Courses" },

        // Admin Menu
        { Options.TotalSalaryPaidByDepartment, "Total Salary Paid By Department" },
        { Options.MedianSalaryByDepartment, "Median Salary By Department" },
        
        

        // Employee Menu
        { Options.ViewAllEmployees, "View All Employees" },
        { Options.ViewEmployeesByDepartment, "View Employees By Department" },
        { Options.AddNewEmployee, "Add New Employee" },
        
        // Student Menu
        { Options.ViewAllStudents, "View All Students" },
        { Options.AddNewStudent, "Add New Student" },
        { Options.SearchStudentById, "Search Student By ID" },
        { Options.EnrollStudent, "Enroll Student" },
        { Options.SetStudentGrade, "Set Student Grade" },
        
        // Course Menu
        { Options.ViewAllActiveCourses, "View All Active Courses" },
        { Options.ViewAllGrades, "View All Grades" },

        // Other
        { Options.Exit, "Exit" },
        { Options.Back, "Back" }
    };

    // Get the text for a specific option
    private static string GetText(Options option)
    {
        return Texts[option];
    }

    // Array of all main menu options (Add more options here)
    public static Options[] GetMainMenuOptions()
    {
        Options[] option = new Options[5];
        option[0] = Options.AdminMenu;
        option[1] = Options.EmployeeMenu;
        option[2] = Options.StudentMenu;
        option[3] = Options.CourseMenu;
        option[4] = Options.Exit;
        return option;
    }

    // Array of all admin menu options (Add more options here)
    public static Options[] GetAdminMenuOptions()
    {
        Options[] option = new Options[3];
        option[0] = Options.TotalSalaryPaidByDepartment;
        option[1] = Options.MedianSalaryByDepartment;
        option[2] = Options.Back;
        return option;
    }

    // Array of all employee menu options (Add more options here)
    public static Options[] GetEmployeeMenuOptions()
    {
        Options[] option = new Options[4];
        option[0] = Options.ViewAllEmployees;
        option[1] = Options.ViewEmployeesByDepartment;
        option[2] = Options.AddNewEmployee;
        option[3] = Options.Back;
        return option;
    }

    // Array of all student menu options (Add more options here)
    public static Options[] GetStudentMenuOptions()
    {
        Options[] option = new Options[6];
        option[0] = Options.ViewAllStudents;
        option[1] = Options.AddNewStudent;
        option[2] = Options.SearchStudentById;
        option[3] = Options.EnrollStudent;
        option[4] = Options.SetStudentGrade;
        option[5] = Options.Back;
        return option;
    }
    
    // Array of all course menu options (Add more options here)
    public static Options[] GetCourseMenuOptions()
    {
        Options[] option = new Options[3];
        option[0] = Options.ViewAllActiveCourses;
        option[1] = Options.ViewAllGrades;
        option[2] = Options.Back;
        return option;
    }

    // Show the menu and return the selected option
    public static Options ShowMenu(string title, Menu.Options[] options)
    {
        while (true) // Loop until a valid option is selected
        {
            Console.Clear();
            Logo.PrintAppLogo();
            Console.WriteLine(title);
            Console.WriteLine();

            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {GetText(options[i])}");
            }
            
            var choice = GetMenuChoice(options.Length);
            if (choice != -1)
            {
                return options[(int)(choice - 1)];
            }

            InvalidOption();
        }
    }
    
    // Get the user's choice from the menu
    public static int? GetMenuChoice(int? maxOptions = null)
    {
        Console.WriteLine();
        Console.Write("[ ]");
        Console.CursorVisible = true;
        Console.SetCursorPosition(1, Console.CursorTop);
        var keyInfo = Console.ReadKey(true);
        Console.SetCursorPosition(0, Console.CursorTop);
        return ValidateOption(keyInfo, maxOptions ?? 0);
    }

    // Validate the user input
    private static int? ValidateOption(ConsoleKeyInfo keyInfo, int? max)
    {
        if (int.TryParse(keyInfo.KeyChar.ToString(), out int choice) && (max == null || (choice > 0 && choice <= max)))
        {
            return choice;
        }

        return null;
    }
    
    // Error message for invalid option
    public static void InvalidOption()
    {
        Console.WriteLine("Invalid option. Please try again.");
        Console.CursorVisible = false;
        Thread.Sleep(600);
    }
}