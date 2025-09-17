using HabitLogger.Models;
using Spectre.Console;

namespace HabitLogger.Views;

public static class UserInterface
{
    public static void ViewAllHabits(List<Habit> habits)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Name");
        table.AddColumn("Measurement Unit");
        foreach (var habit in habits)
            table.AddRow(habit.Id.ToString(), habit.Name, habit.UnitOfMeasurement);

        AnsiConsole.Write(table);
    }

    public static void ViewAllProgress(List<HabitProgress> progressList)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Date");
        table.AddColumn("Quantity");
        table.AddColumn("Habit Name");

        foreach (var progress in progressList)
            table.AddRow(
                progress.Id.ToString(),
                progress.Date.ToString("D"),
                $"{progress.Quantity} {progress.Habit.UnitOfMeasurement}",
                progress.Habit.Name
            );

        AnsiConsole.Write(table);
    }
}
