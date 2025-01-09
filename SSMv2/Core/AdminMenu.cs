using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using SSMv2.Models;
namespace SSMv2;

public class TotalSalaryByDepartmentResult
{
    public string DepartmentName { get; set; }
    public decimal TotalSalaryPayment { get; set; }
}

public class MedianSalaryByDepartmentResult
{
    public string DepartmentName { get; set; }
    public double MedianSalary { get; set; }
}

public class AdminMenu
{
    public static void ShowMenu()
    {
        const bool isRunning = true;
        while (isRunning)
        {
            var choice = Menu.ShowMenu("| Admin Menu |", Menu.GetAdminMenuOptions());
            switch (choice)
            {
                case Menu.Options.TotalSalaryPaidByDepartment:
                    TotalSalaryPaidByDepartment();
                    break;
                case Menu.Options.MedianSalaryByDepartment:
                    MedianSalaryByDepartment();
                    break;
                case Menu.Options.Back:
                    return;
            }
        }
    }
    
    private static void TotalSalaryPaidByDepartment()
    {
        Console.Clear();
        using var db = new AppDbContext();
        var results = db.TotalSalaryByDepartmentResults.FromSql($"EXEC CalculateTotalSalaryPayment").ToList();

        Console.WriteLine("| Total Salary Paid By Department |\n");
        Console.WriteLine($"Department".PadRight(20) + $"Total Salary".PadRight(20));

        foreach (var result in results)
        {
            Console.WriteLine($"{result.DepartmentName.PadRight(20)}{result.TotalSalaryPayment.ToString().PadRight(20)}");
        }

        Console.WriteLine("-----------------------------");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static void MedianSalaryByDepartment()
    {
        Console.Clear();
        using var db = new AppDbContext();
        var results = db.MedianSalaryByDepartmentResults.FromSql($"EXEC CalculateMedianSalaryByDepartment").ToList();

        Console.WriteLine("| Median Salary By Department |\n");
        Console.WriteLine($"Department".PadRight(20) + $"Median Salary".PadRight(20));

        foreach (var result in results)
        {
            Console.WriteLine($"{result.DepartmentName.PadRight(20)}{result.MedianSalary.ToString().PadRight(20)}");
        }

        Console.WriteLine("-----------------------------");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}

