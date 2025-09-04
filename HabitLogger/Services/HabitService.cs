using HabitLogger.Data;
using HabitLogger.Models;
using HabitLogger.Utils;
using HabitLogger.Views;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace HabitLogger.Services;

public class HabitService : IHabitService
{
    private readonly DatabaseContext _databaseContext;

    public HabitService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
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

        using (var connection = _databaseContext.ConnectionString)
        {
            connection.Open();

            using (var tableCommand = connection.CreateCommand())
            {
                tableCommand.CommandText =
                    $"INSERT INTO habits(Name, MeasurementUnit) VALUES ('{name}', '{measurementUnit}')";

                tableCommand.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void GetHabits()
    {
        List<Habit> habits = new();

        using (var connection = _databaseContext.ConnectionString)
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "SELECT * FROM habits";

            using (var reader = tableCmd.ExecuteReader())
            {
                if (reader.HasRows)
                    while (reader.Read())
                        try
                        {
                            var habit = new Habit
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                UnitOfMeasurement = reader.GetString(2),
                            };
                            habits.Add(habit);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error getting record: {ex.Message}. ");
                        }
                else
                    Console.WriteLine("No rows found");
            }
        }

        UserInterface.ViewAllHabits(habits);
    }

    public void DeleteHabit()
    {
        GetHabits();

        var id = Helpers.GetNumberInput("Please type the id of the habit you want to delete.");

        using (var connection = _databaseContext.ConnectionString)
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = @$"DELETE FROM habits WHERE Id = {id}";

                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void UpdateHabit()
    {
        GetHabits();

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

        string query;
        if (updateName && updateUnit)
            query =
                $"UPDATE habits SET Name = '{name}', MeasurementUnit = '{unit}' WHERE Id = {id}";
        else if (updateName && !updateUnit)
            query = $"UPDATE habits SET Name = '{name}' WHERE Id = {id}";
        else
            query = $"UPDATE habits SET MeasurementUnit = '{unit}' WHERE Id = {id}";

        using (var connection = _databaseContext.ConnectionString)
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = query;

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}
