namespace SSMv2;

public class App
{
    public static void Start()
    {
        var isRunning = true;
        while (isRunning)
        {
            // Set the console title and display the main menu
            Console.Title = "School System Manager";
            var choice = Menu.ShowMenu("| Main Menu |", Menu.GetMainMenuOptions());
            switch (choice)
            {
                case Menu.Options.AdminMenu:
                    AdminMenu.ShowMenu();
                    break;
                case Menu.Options.EmployeeMenu:
                    EmployeeMenu.ShowMenu();
                    break;
                case Menu.Options.StudentMenu:
                    StudentMenu.ShowMenu();
                    break;
                case Menu.Options.CourseMenu:
                    CourseMenu.ShowMenu();
                    break;
                case Menu.Options.Exit:
                    Console.WriteLine("Exiting...");
                    Thread.Sleep(500);
                    return;
            }
        }
    }
}