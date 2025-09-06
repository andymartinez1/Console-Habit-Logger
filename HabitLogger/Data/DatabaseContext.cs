using System.Data;
using Dapper;
using HabitLogger.Utils;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace HabitLogger.Data;

public class DatabaseContext
{
    internal readonly IDbConnection ConnectionString;

    public DatabaseContext(IDbConnection connectionString)
    {
        ConnectionString = connectionString;
    }

    internal void CreateDatabase()
    {
        using (var connection = ConnectionString)
        {
            connection.Open();

            var createProgressTableQuery =
                @"CREATE TABLE IF NOT EXISTS progress (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER,
                        HabitId INTEGER,
                        FOREIGN KEY(habitId) REFERENCES habits(Id) ON DELETE CASCADE
                        )";

            connection.Execute(createProgressTableQuery);

            var createHabitTableQuery =
                @"CREATE TABLE IF NOT EXISTS habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        MeasurementUnit TEXT
                        )";

            connection.Execute(createHabitTableQuery);

            // Seed the database with initial data if needed
            SeedData();
        }
    }

    private void SeedData()
    {
        var recordsTableEmpty = IsTableEmpty("progress");
        var habitsTableEmpty = IsTableEmpty("habits");

        if (!recordsTableEmpty || !habitsTableEmpty)
            return;

        string[] habitNames =
        {
            "Walking",
            "Biking",
            "Daily Pushups",
            "Reading",
            "Water Consumption",
        };
        string[] habitUnits = { "Miles", "Miles", "Repetitions", "Pages", "Ounces" };
        var dates = Helpers.GenerateRandomDates(100);
        var quantities = Helpers.GenerateRandomQuantities(100, 0, 200);

        using (var connection = ConnectionString)
        {
            connection.Open();

            for (var i = 0; i < habitNames.Length; i++)
            {
                var insertHabitQuery =
                    "INSERT INTO habits (Name, MeasurementUnit) VALUES (@Name, @MeasurementUnit);";
                var command = new SqliteCommand(insertHabitQuery);
                command.Parameters.AddWithValue("@Name", habitNames[i]);
                command.Parameters.AddWithValue("@MeasurementUnit", habitUnits[i]);

                connection.Execute(insertHabitQuery);
            }

            for (var i = 0; i < 100; i++)
            {
                var insertProgressQuery =
                    "INSERT INTO progress (Date, Quantity, HabitId) VALUES (@Date, @Quantity, @HabitId);";
                var command = new SqliteCommand(insertProgressQuery);
                command.Parameters.AddWithValue("@Date", dates[i]);
                command.Parameters.AddWithValue("@Quantity", quantities[i]);
                command.Parameters.AddWithValue("@HabitId", Helpers.GetRandomHabitId());

                connection.Execute(insertProgressQuery);
            }
        }
    }

    private bool IsTableEmpty(string tableName)
    {
        using (var connection = ConnectionString)
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                var count = connection.ExecuteScalar<int>($"SELECT COUNT(*) FROM {tableName}");

                return count == 0;
            }
        }
    }
}
