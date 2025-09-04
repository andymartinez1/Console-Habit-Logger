using HabitLogger.Services;
using Spectre.Console;

namespace HabitLogger.Views
{
    internal class Menu
    {
        internal static void MainMenu()
        {
            var isMenuRunning = true;

            while (isMenuRunning)
            {
                var usersChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Welcome! Please select from the following options:")
                        .AddChoices(
                            "Add Habit",
                            "Delete Habit",
                            "Update Habit",
                            "Add Progress",
                            "Delete Progress",
                            "View All Progress",
                            "Update Progress",
                            "Quit"
                        )
                );

                switch (usersChoice)
                {
                    case "Add Habit":
                        HabitService.InsertHabit();
                        break;
                    case "Delete Habit":
                        HabitService.DeleteHabit();
                        break;
                    case "Update Habit":
                        HabitService.UpdateHabit();
                        break;
                    case "Add Progress":
                        HabitService.InsertProgress();
                        break;
                    case "Delete Progress":
                        HabitService.DeleteProgress();
                        break;
                    case "View All Progress":
                        HabitService.GetProgress();
                        break;
                    case "Update Progress":
                        HabitService.UpdateProgress();
                        break;
                    case "Quit":
                        Console.WriteLine("Goodbye");
                        isMenuRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please choose one of the above");
                        break;
                }
            }
        }
    }
}
