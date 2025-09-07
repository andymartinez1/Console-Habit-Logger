using HabitLogger.Repository;
using HabitLogger.Utils;
using Spectre.Console;

namespace HabitLogger.Services;

public class HabitService : IHabitService
{
    private readonly HabitRepository _habitRepository;

    public HabitService(HabitRepository habitRepository)
    {
        _habitRepository = habitRepository;
    }

    public void InsertHabit()
    {
        var name = AnsiConsole.Ask<string>("Enter the name of the habit:");
        while (string.IsNullOrWhiteSpace(name))
            name = AnsiConsole.Ask<string>(
                "Habit name cannot be empty. Enter the name of the habit:"
            );

        var measurementUnit = AnsiConsole.Ask<string>(
            "Enter the measurement unit for the habit (e.g., 'pushups', 'minutes', etc.):"
        );

        while (string.IsNullOrWhiteSpace(measurementUnit))
            measurementUnit = AnsiConsole.Ask<string>(
                "Measurement unit cannot be empty. Enter the measurement unit for the habit:"
            );
    }

    public void GetHabits() { }

    public void UpdateHabit()
    {
        var id = Helpers.GetNumberInput("Please type the id of the habit you want to update.");

        var name = "";
        var updateName = AnsiConsole.Confirm("Update name?");
        if (updateName)
        {
            name = AnsiConsole.Ask<string>("Habit's new name:");
            while (string.IsNullOrEmpty(name))
                name = AnsiConsole.Ask<string>("Habit's name can't be empty. Try again:");
        }

        var unit = "";
        var updateUnit = AnsiConsole.Confirm("Update Unit of Measurement?");
        if (updateUnit)
        {
            unit = AnsiConsole.Ask<string>("Habit's Unit of Measurement:");
            while (string.IsNullOrEmpty(unit))
                unit = AnsiConsole.Ask<string>("Habit's unit can't be empty. Try again:");
        }
    }

    public void DeleteHabit() { }
}
