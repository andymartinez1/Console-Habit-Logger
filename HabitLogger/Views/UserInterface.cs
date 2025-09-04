using HabitLogger.Models;
using Spectre.Console;
using Progress = HabitLogger.Models.Progress;

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

    public static void ViewAllProgress(List<Progress> progress)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Date");
        table.AddColumn("Quantity");
        table.AddColumn("Habit Name");

        foreach (var record in progress)
            table.AddRow(
                record.Id.ToString(),
                record.Date.ToString("D"),
                $"{record.Quantity} {record.UnitOfMeasurement}",
                record.HabitName
            );

        AnsiConsole.Write(table);
    }
}
