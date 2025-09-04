using System.Globalization;

namespace HabitLogger.Utils;

public class Helpers
{
    public static string GetDateInput(string message)
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
            Console.WriteLine("Invalid date format. Please enter the date in mm-dd-yyyy format.");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    public static int GetNumberInput(string message)
    {
        Console.WriteLine(message);
        var numberInput = Console.ReadLine();

        var output = 0;
        while (!int.TryParse(numberInput, out output) || output < 0)
        {
            Console.WriteLine("Invalid number. Try again");
            numberInput = Console.ReadLine();
        }

        return output;
    }

    public static int[] GenerateRandomQuantities(int count, int min, int max)
    {
        var random = new Random();
        var quantities = new int[count];

        for (var i = 0; i < count; i++)
            quantities[i] = random.Next(min, max + 1);

        return quantities;
    }

    public static string[] GenerateRandomDates(int count)
    {
        var startDate = new DateTime(2025, 1, 1);
        var range = DateTime.Today - startDate;

        var randomDateStrings = new string[count];
        var random = new Random();

        for (var i = 0; i < count; i++)
        {
            var daysToAdd = random.Next(0, range.Days);
            var randomDate = startDate.AddDays(daysToAdd);
            randomDateStrings[i] = randomDate.ToString("MM-dd-yyyy");
        }

        return randomDateStrings;
    }

    public static int GetRandomHabitId()
    {
        var random = new Random();
        return random.Next(1, 6);
    }
}
