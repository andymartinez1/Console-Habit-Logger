using HabitLogger.Models;
using HabitLogger.Repository;
using HabitLogger.Utils;
using HabitLogger.Views;
using Spectre.Console;

namespace HabitLogger.Services;

public class HabitService : IHabitService
{
    private readonly IHabitRepository _habitRepository;

    public HabitService(IHabitRepository habitRepository)
    {
        _habitRepository = habitRepository;
    }

    public void InsertHabit()
    {
        var habit = new Habit();

        habit.Name = AnsiConsole.Ask<string>("Enter the name of the habit:");
        while (string.IsNullOrWhiteSpace(habit.Name))
            habit.Name = AnsiConsole.Ask<string>(
                "Habit name cannot be empty. Enter the name of the habit:"
            );

        habit.UnitOfMeasurement = AnsiConsole.Ask<string>(
            "Enter the measurement unit for the habit (e.g., 'pushups', 'minutes', etc.):"
        );

        while (string.IsNullOrWhiteSpace(habit.UnitOfMeasurement))
            habit.UnitOfMeasurement = AnsiConsole.Ask<string>(
                "Measurement unit cannot be empty. Enter the measurement unit for the habit:"
            );

        _habitRepository.InsertHabit(habit);
    }

    public void GetHabits()
    {
        var habits = _habitRepository.GetHabits();

        if (habits.Any())
            UserInterface.ViewAllHabits(habits);
        else
            AnsiConsole.MarkupLine("[Red]No habits to display. Please add a new habit[/]");
    }

    public Habit GetHabitById()
    {
        GetHabits();

        var id = UserInputValidation.ValidateNumberInput("Select the ID of the Habit");

        return _habitRepository.GetHabitById(id);
    }

    public void UpdateHabit()
    {
        var habit = GetHabitById();

        if (habit == null)
        {
            AnsiConsole.MarkupLine(
                "[Red]No habit found. Please select an existing habit from the list.[/]"
            );
            return;
        }

        var updateName = AnsiConsole.Confirm("Update name?");
        if (updateName)
        {
            habit.Name = AnsiConsole.Ask<string>("Habit's new name:");
            while (string.IsNullOrEmpty(habit.Name))
                habit.Name = AnsiConsole.Ask<string>("Habit's name can't be empty. Try again:");
        }
        else
        {
            habit.Name = habit.Name;
        }

        var updateUnit = AnsiConsole.Confirm("Update Unit of Measurement?");
        if (updateUnit)
        {
            habit.UnitOfMeasurement = AnsiConsole.Ask<string>("Habit's Unit of Measurement:");
            while (string.IsNullOrEmpty(habit.UnitOfMeasurement))
                habit.UnitOfMeasurement = AnsiConsole.Ask<string>(
                    "Habit's unit can't be empty. Try again:"
                );
        }

        _habitRepository.UpdateHabit(habit);
    }

    public void DeleteHabit()
    {
        var habit = GetHabitById();

        if (habit == null)
        {
            AnsiConsole.MarkupLine(
                "[Red]No habit found. Please select an existing habit from the list.[/]"
            );
            return;
        }

        var id = habit.Id;

        _habitRepository.DeleteHabit(id);
    }
}
