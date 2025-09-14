using System.Globalization;
using HabitLogger.Models;
using Spectre.Console;
using Progress = HabitLogger.Models.Progress;

namespace HabitLogger.Utils;

public static class UserInputValidation
{
    public static string ValidateDateInput(string message)
    {
        Console.WriteLine(message);
        var dateInput = Console.ReadLine();

        while (
            !DateTime.TryParseExact(
                dateInput,
                "MM-dd-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _
            )
        )
        {
            AnsiConsole.MarkupLine(
                "[Red]Invalid date format. Please enter the date in mm-dd-yyyy format.[/]"
            );
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    public static int ValidateNumberInput(string message)
    {
        Console.WriteLine(message);
        var numberInput = Console.ReadLine();

        var output = 0;
        while (!int.TryParse(numberInput, out output) || output < 0)
        {
            AnsiConsole.MarkupLine("[Red]Invalid number. Try again[/]");
            numberInput = Console.ReadLine();
        }

        return output;
    }

    public static bool IsHabitValid(Habit habit)
    {
        if (habit == null)
        {
            AnsiConsole.MarkupLine(
                "[Red]No habit found. Please select an existing habit from the list.[/]"
            );
            return false;
        }

        return true;
    }

    public static bool IsProgressValid(Progress progress)
    {
        if (progress == null)
        {
            AnsiConsole.MarkupLine(
                "[Red]No progress found. Please select an existing habit progress from the list.[/]"
            );
            return false;
        }

        return true;
    }
}
