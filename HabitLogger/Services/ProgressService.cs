using System.Globalization;
using HabitLogger.Data;
using HabitLogger.Utils;
using HabitLogger.Views;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using Progress = HabitLogger.Models.Progress;

namespace HabitLogger.Services;

public class ProgressService : IProgressService
{
    private readonly DatabaseContext _databaseContext;
    private readonly IHabitService _habitService;

    public ProgressService(IHabitService habitService, DatabaseContext databaseContext)
    {
        _habitService = habitService;
        _databaseContext = databaseContext;
    }

    public void InsertProgress()
    {
        var date = Helpers.GetDateInput(
            "Enter the date. (Format: mm-dd-yyyy). Type 0 to return to the main menu."
        );

        _habitService.GetHabits();

        var habitId = Helpers.GetNumberInput(
            "Enter the ID of the habit for which you want to add a record. Type 0 to return to the main menu."
        );
        var quantity = Helpers.GetNumberInput("Enter quantity. Type 0 to return to the main menu.");

        Console.Clear();

        using (var connection = new SqliteConnection(_databaseContext.ConnectionString))
        {
            connection.Open();

            using (var tableCommand = connection.CreateCommand())
            {
                tableCommand.CommandText =
                    $"INSERT INTO progress(date, quantity, habitId) VALUES ('{date}', {quantity}, {habitId})";

                tableCommand.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void GetProgress()
    {
        List<Progress> records = new();

        using (var connection = new SqliteConnection(_databaseContext.ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText =
                @"
                    SELECT progress.Id, progress.Date, progress.Quantity, progress.HabitId, habits.Name AS HabitName, habits.UnitOfMeasurement
                    FROM progress
                    INNER JOIN habits ON progress.HabitId = habits.Id";

            using (var reader = tableCommand.ExecuteReader())
            {
                if (reader.HasRows)
                    while (reader.Read())
                        try
                        {
                            var progress = new Progress
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(
                                    reader.GetString(1),
                                    "dd-MM-yyyy",
                                    CultureInfo.InvariantCulture
                                ),
                                Quantity = reader.GetInt32(2),
                                HabitName = reader.GetString(4),
                                UnitOfMeasurement = reader.GetString(5),
                            };
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing record: {ex.Message}");
                        }
                else
                    Console.WriteLine("No progress found.");
            }
        }

        UserInterface.ViewAllProgress(records);
    }

    public void DeleteProgress()
    {
        GetProgress();

        var id = Helpers.GetNumberInput(
            "Enter the ID of the record you want to delete. Type 0 to return to the main menu."
        );

        using (var connection = new SqliteConnection(_databaseContext.ConnectionString))
        {
            connection.Open();

            using (var tableCommand = connection.CreateCommand())
            {
                tableCommand.CommandText = $"DELETE FROM progress WHERE Id = {id}";

                var rowsAffected = tableCommand.ExecuteNonQuery();
                if (rowsAffected != 0)
                    Console.WriteLine($"Record with ID {id} deleted successfully.");
            }

            connection.Close();
        }
    }

    public void UpdateProgress()
    {
        GetProgress();

        var id = Helpers.GetNumberInput(
            "Enter the ID of the record you want to update. Type 0 to return to the main menu."
        );

        var date = "";
        var updateDate = AnsiConsole.Confirm("Do you want to update the date?");
        if (updateDate)
            date = Helpers.GetDateInput(
                "Enter the new date. (Format: mm-dd-yyyy). Type 0 to return to the main menu."
            );

        var quantity = 0;
        var updateQuantity = AnsiConsole.Confirm("Do you want to update the quantity?");
        if (updateQuantity)
            quantity = Helpers.GetNumberInput(
                "Enter the new quantity. Type 0 to return to the main menu."
            );

        string query;
        if (updateDate && updateQuantity)
            query = $"UPDATE progress SET Date = '{date}', Quantity = {quantity} WHERE Id = {id}";
        else if (updateDate && !updateQuantity)
            query = $"UPDATE progress SET Date = '{date}' WHERE Id = {id}";
        else
            query = $"UPDATE progress SET Quantity = {quantity} WHERE Id = {id}";

        using (var connection = new SqliteConnection(_databaseContext.ConnectionString))
        {
            connection.Open();

            using (var tableCommand = connection.CreateCommand())
            {
                tableCommand.CommandText = query;

                tableCommand.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
