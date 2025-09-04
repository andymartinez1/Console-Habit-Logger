using System.Data;
using HabitLogger.Utils;
using Microsoft.Data.Sqlite;

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
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();

                tableCommand.CommandText =
                    @"CREATE TABLE IF NOT EXISTS progress (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER,
                        HabitId INTEGER,
                        FOREIGN KEY(habitId) REFERENCES habits(Id) ON DELETE CASCADE
                        )";

                tableCommand.ExecuteNonQuery();

                tableCommand.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        MeasurementUnit TEXT
                        )";

                tableCommand.ExecuteNonQuery();
            }
        }

        // Seed the database with initial data if needed
        SeedData();
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
                var insertSql =
                    "INSERT INTO habits (Name, MeasurementUnit) VALUES (@Name, @MeasurementUnit);";
                var command = new SqliteCommand(insertSql);
                command.Parameters.AddWithValue("@Name", habitNames[i]);
                command.Parameters.AddWithValue("@MeasurementUnit", habitUnits[i]);

                command.ExecuteNonQuery();
            }

            for (var i = 0; i < 100; i++)
            {
                var insertSql =
                    "INSERT INTO progress (Date, Quantity, HabitId) VALUES (@Date, @Quantity, @HabitId);";
                var command = new SqliteCommand(insertSql);
                command.Parameters.AddWithValue("@Date", dates[i]);
                command.Parameters.AddWithValue("@Quantity", quantities[i]);
                command.Parameters.AddWithValue("@HabitId", Helpers.GetRandomHabitId());

                command.ExecuteNonQuery();
            }
        }
    }

    internal bool IsTableEmpty(string tableName)
    {
        using (var connection = ConnectionString)
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT COUNT(*) FROM {tableName}";
                var count = (long)command.ExecuteScalar();

                return count == 0;
            }
        }
    }
}
