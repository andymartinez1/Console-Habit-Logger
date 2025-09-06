using HabitLogger.Enums;
using HabitLogger.Services;
using HabitLogger.Utils;
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

    private readonly MenuOptions[] _menuOptions =
    [
        MenuOptions.AddHabit,
        MenuOptions.DeleteHabit,
        MenuOptions.UpdateHabit,
        MenuOptions.AddProgress,
        MenuOptions.DeleteProgress,
        MenuOptions.ViewAllProgress,
        MenuOptions.UpdateProgress,
        MenuOptions.Quit,
    ];

    public void MainMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                    .Title("Welcome! Please select from the following options:")
                    .AddChoices(_menuOptions)
                    .UseConverter(c => c.GetDisplayName())
            );

            switch (usersChoice)
            {
                case MenuOptions.AddHabit:
                    _habitService.InsertHabit();
                    break;
                case MenuOptions.DeleteHabit:
                    _habitService.DeleteHabit();
                    break;
                case MenuOptions.UpdateHabit:
                    _habitService.UpdateHabit();
                    break;
                case MenuOptions.AddProgress:
                    _progressService.InsertProgress();
                    break;
                case MenuOptions.DeleteProgress:
                    _progressService.DeleteProgress();
                    break;
                case MenuOptions.ViewAllProgress:
                    _progressService.GetProgress();
                    break;
                case MenuOptions.UpdateProgress:
                    _progressService.UpdateProgress();
                    break;
                case MenuOptions.Quit:
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
