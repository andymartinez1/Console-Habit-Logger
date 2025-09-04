using HabitLogger.Services;
using Spectre.Console;

namespace HabitLogger.Views;

public class Menu : IMenu
{
    private readonly IHabitService _habitService;
    private readonly IProgressService _progressService;

    public Menu(IHabitService habitService, IProgressService progressService)
    {
        _habitService = habitService;
        _progressService = progressService;
    }

    public void MainMenu()
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
                    _habitService.InsertHabit();
                    break;
                case "Delete Habit":
                    _habitService.DeleteHabit();
                    break;
                case "Update Habit":
                    _habitService.UpdateHabit();
                    break;
                case "Add Progress":
                    _progressService.InsertProgress();
                    break;
                case "Delete Progress":
                    _progressService.DeleteProgress();
                    break;
                case "View All Progress":
                    _progressService.GetProgress();
                    break;
                case "Update Progress":
                    _progressService.UpdateProgress();
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
