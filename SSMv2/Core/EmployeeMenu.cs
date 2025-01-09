using Microsoft.EntityFrameworkCore;
using SSMv2.Models;

namespace SSMv2;

public class EmployeeMenu
{
    public static void ShowMenu()
    {
        const bool isRunning = true;
        while (isRunning)
        {
            var choice = Menu.ShowMenu("| Employee Menu |", Menu.GetEmployeeMenuOptions());
            switch (choice)
            {
                case Menu.Options.ViewAllEmployees:
                    ViewAllEmployees();
                    break;
                case Menu.Options.ViewEmployeesByDepartment:
                    ViewEmployeesByDepartment();
                    break;
                case Menu.Options.AddNewEmployee:
                    AddNewEmployee();
                    break;
                case Menu.Options.Back:
                    return;
            }
        }
    }
    
    // View all employees
    private static void ViewAllEmployees()
    {
        Console.Clear();
        using var db = new AppDbContext();
        var employees = db.EmployeeDetails.ToList();
    
        // Print the header with the appropriate padding
        Console.WriteLine("| All Employees |\n");
        Console.WriteLine($"ID".PadRight(5) +
                          $"Name".PadRight(25) +
                          $"Position".PadRight(20) +
                          $"Department".PadRight(20) +
                          $"Years Employed".PadRight(20));
    
        // Print each employee with the appropriate padding
        foreach (var employee in employees)
        {
            Console.WriteLine($"{employee.EmployeeID.ToString().PadRight(5)}" +
                              $"{(employee.FirstName + " " + employee.LastName).PadRight(25)}" +
                              $"{employee.Position.PadRight(20)}" +
                              $"{employee.DepartmentName.PadRight(20)}" +
                              $"{employee.YearsEmployed.ToString().PadRight(20)}");
        }

        Console.WriteLine("-----------------------------");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }


    // View how many employees exist in each department
    private static void ViewEmployeesByDepartment()
    {
        Console.Clear();
        using var db = new AppDbContext();
        var employees = db.Employees.Include(e => e.DepartmentNavigation).ToList();
    
        Console.WriteLine("| Employees By Department |\n");
        Console.WriteLine("Department".PadRight(20) + "Count".PadRight(5));
        
        var departments = employees.GroupBy<Models.Employee, object>(e => e.DepartmentNavigation.DepartmentName)
            .Select(g => new { Department = g.Key, Count = g.Count() });
    
        foreach (var department in departments)
        {
            Console.WriteLine($"{department.Department.ToString().PadRight(20)}" +
                              $"{department.Count.ToString().PadRight(5)}");
        }
    
        Console.WriteLine("-----------------------------");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    
    // Add new employee
    private static void AddNewEmployee()
    {
        Console.Clear();
        Console.WriteLine("| Add New Employee |");
        Console.WriteLine("Enter first name:");
        var firstName = Console.ReadLine();
        Console.WriteLine("Enter last name:");
        var lastName = Console.ReadLine();
        Console.WriteLine("Name of position:");
        var position = Console.ReadLine();
        
        var isDone = false;
        while (!isDone)
        {
            Console.WriteLine("[1] Management");
            Console.WriteLine("[2] HR");
            Console.WriteLine("[3] IT");
            Console.WriteLine("[4] Staff");
            Console.WriteLine("Enter department ID:");
            var choice = Menu.GetMenuChoice(4);

            if (choice is 1 or 2 or 3 or 4)
            {
                using var db = new AppDbContext();
                var department = db.Departments.Find(choice);
                if (department != null)
                {
                    var employee = new Models.Employee
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Position = position,
                        Department = (int)choice,
                        StartDate = DateOnly.FromDateTime(DateTime.Now)
                    };
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    Console.Clear();
                    Console.WriteLine("Employee added successfully!");
                    Thread.Sleep(1000);
                    isDone = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Department not found. Try again.");
                    Thread.Sleep(1000);
                }
            }
            else
            {
                Console.Clear();
                Menu.InvalidOption();
            }
        }
    }
}