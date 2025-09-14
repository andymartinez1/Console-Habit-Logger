using HabitLogger.Enums;
using HabitLogger.Services;
using HabitLogger.Utils;
using Spectre.Console;

namespace HabitLogger.Views;

public class Menu : IMenu
{
    private readonly HabitMenuOptions[] _habitMenuOptions =
    [
        HabitMenuOptions.AddHabit,
        HabitMenuOptions.ViewAllHabits,
        HabitMenuOptions.UpdateHabit,
        HabitMenuOptions.DeleteHabit,
        HabitMenuOptions.BackToMainMenu,
    ];

    private readonly IHabitService _habitService;

    private readonly MainMenuOptions[] _mainMenuOptions =
    [
        MainMenuOptions.ManageHabits,
        MainMenuOptions.ManageProgress,
        MainMenuOptions.Quit,
    ];

    private readonly ProgressMenuOptions[] _progressMenuOptions =
    [
        ProgressMenuOptions.AddProgress,
        ProgressMenuOptions.ViewAllProgress,
        ProgressMenuOptions.UpdateProgress,
        ProgressMenuOptions.DeleteProgress,
        ProgressMenuOptions.BackToMainMenu,
    ];

    private readonly IProgressService _progressService;

    public Menu(IHabitService habitService, IProgressService progressService)
    {
        _habitService = habitService;
        _progressService = progressService;
    }

    public void HabitMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<HabitMenuOptions>()
                    .Title("Welcome! Please select from the following options:")
                    .AddChoices(_habitMenuOptions)
                    .UseConverter(c => c.GetDisplayName())
            );

            switch (usersChoice)
            {
                case HabitMenuOptions.AddHabit:
                    AnsiConsole.Clear();
                    _habitService.InsertHabit();
                    break;
                case HabitMenuOptions.ViewAllHabits:
                    AnsiConsole.Clear();
                    _habitService.GetHabits();
                    break;
                case HabitMenuOptions.UpdateHabit:
                    AnsiConsole.Clear();
                    _habitService.UpdateHabit();
                    break;
                case HabitMenuOptions.DeleteHabit:
                    AnsiConsole.Clear();
                    _habitService.DeleteHabit();
                    break;
                case HabitMenuOptions.BackToMainMenu:
                    AnsiConsole.Clear();
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please choose one of the above");
                    break;
            }
        }
    }

    public void MainMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOptions>()
                    .Title("Welcome! Please select from the following options:")
                    .AddChoices(_mainMenuOptions)
                    .UseConverter(c => c.GetDisplayName())
            );

            switch (usersChoice)
            {
                case MainMenuOptions.ManageHabits:
                    AnsiConsole.Clear();
                    HabitMenu();
                    break;
                case MainMenuOptions.ManageProgress:
                    AnsiConsole.Clear();
                    ProgressMenu();
                    break;
                case MainMenuOptions.Quit:
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine("[red]Thank you for using the Habit Logger app[/]");
                    isMenuRunning = false;
                    Environment.Exit(0);
                    break;
            }
        }
    }

    public void ProgressMenu()
    {
        var isMenuRunning = true;

        while (isMenuRunning)
        {
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<ProgressMenuOptions>()
                    .Title("Welcome! Please select from the following options:")
                    .AddChoices(_progressMenuOptions)
                    .UseConverter(c => c.GetDisplayName())
            );

            switch (usersChoice)
            {
                case ProgressMenuOptions.AddProgress:
                    AnsiConsole.Clear();
                    _progressService.InsertProgress();
                    break;
                case ProgressMenuOptions.ViewAllProgress:
                    AnsiConsole.Clear();
                    _progressService.GetProgress();
                    break;
                case ProgressMenuOptions.UpdateProgress:
                    AnsiConsole.Clear();
                    _progressService.UpdateProgress();
                    break;
                case ProgressMenuOptions.DeleteProgress:
                    AnsiConsole.Clear();
                    _progressService.DeleteProgress();
                    break;
                case ProgressMenuOptions.BackToMainMenu:
                    AnsiConsole.Clear();
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please choose one of the above");
                    break;
            }
        }
    }
}
