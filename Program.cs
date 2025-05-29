using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;

var connectionString = @"Data Source=habbitlogger.db";

CreateDatabase();

MainMenu();

void CreateDatabase()
{
    using (SqliteConnection connection = new(connectionString))
    {
        using (SqliteCommand tableCommand = connection.CreateCommand())
        {
            connection.Open();

            tableCommand.CommandText =
                @"CREATE TABLE IF NOT EXISTS records (
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
}

void MainMenu()
{
    var isMenuRunning = true;

    while (isMenuRunning)
    {
        var usersChoice = AnsiConsole.Prompt(
               new SelectionPrompt<string>()
                .Title("Please select an option:")
                .AddChoices(
                   "Add Habit",
                   "Delete Habit",
                   "Update Habit",
                   "Add Record",
                   "Delete Record",
                   "View Records",
                   "Update Record",
                   "Quit")
                );

        switch (usersChoice)
        {
            case "Add Habit":
                InsertHabit();
                break;
            case "Delete Habit":
                DeleteHabit();
                break;
            case "Update Habit":
                UpdateHabit();
                break;
            case "Add Record":
                InsertRecord();
                break;
            case "Delete Record":
                DeleteRecord();
                break;
            case "View Records":
                GetRecords();
                break;
            case "Update Record":
                UpdateRecord();
                break;
            case "Quit":
                Console.WriteLine("Goodbye");
                isMenuRunning = false;
                break;
            default:
                Console.WriteLine("Invalid choice. Please choose one of the above");
                break;
        }
    }
}

void InsertHabit()
{
    string name = AnsiConsole.Ask<string>("Enter the name of the habit:");
    while (string.IsNullOrWhiteSpace(name))
    {
        name = AnsiConsole.Ask<string>("Habit name cannot be empty. Enter the name of the habit:");
    }

    string measurementUnit = AnsiConsole.Ask<string>("Enter the measurement unit for the habit (e.g., 'pushups', 'minutes', etc.):");

    while (string.IsNullOrWhiteSpace(measurementUnit))
    {
        measurementUnit = AnsiConsole.Ask<string>("Measurement unit cannot be empty. Enter the measurement unit for the habit:");
    }

    using (var connection = new SqliteConnection(connectionString))
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

void GetHabits()
{
    List<Habit> habits = new();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT * FROM habits";

        using (SqliteDataReader reader = tableCmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                    try
                    {
                        habits.Add(
                        new Habit(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2)
                            )
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error getting record: {ex.Message}. ");
                    }
            }
            else
            {
                Console.WriteLine("No rows found");
            }
        }
    }
}
void DeleteHabit()
{
    GetHabits();

    var id = GetNumberInput("Please type the id of the habit you want to delete.");

    using (var connection = new SqliteConnection(connectionString))
    {
        using (var command = connection.CreateCommand())
        {
            connection.Open();

            command.CommandText =
                    @$"DELETE FROM habits WHERE Id = {id}";

            command.ExecuteNonQuery();
        }

        connection.Close();
    }
}

void UpdateHabit()
{
    GetHabits();

    var id = GetNumberInput("Please type the id of the habit you want to update.");

    string name = "";
    bool updateName = AnsiConsole.Confirm("Update name?");
    if (updateName)
    {
        name = AnsiConsole.Ask<string>("Habit's new name:");
        while (string.IsNullOrEmpty(name))
        {
            name = AnsiConsole.Ask<string>("Habit's name can't be empty. Try again:");
        }
    }

    string unit = "";
    bool updateUnit = AnsiConsole.Confirm("Update Unit of Measurement?");
    if (updateUnit)
    {
        unit = AnsiConsole.Ask<string>("Habit's Unit of Measurement:");
        while (string.IsNullOrEmpty(unit))
        {
            unit = AnsiConsole.Ask<string>("Habit's unit can't be empty. Try again:");
        }
    }

    string query;
    if (updateName && updateUnit)
    {
        query = $"UPDATE habits SET Name = '{name}', MeasurementUnit = '{unit}' WHERE Id = {id}";
    }
    else if (updateName && !updateUnit)
    {
        query = $"UPDATE habits SET Name = '{name}' WHERE Id = {id}";
    }
    else
    {
        query = $"UPDATE habits SET Unit = '{unit}' WHERE Id = {id}";
    }

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = query;

        tableCmd.ExecuteNonQuery();

        connection.Close();
    }
}

void InsertRecord()
{
    string date = GetDateInput("Enter the date. (Format: mm-dd-yyyy). Type 0 to return to the main menu.");

    GetHabits();

    var habitId = GetNumberInput("Enter the ID of the habit for which you want to add a record. Type 0 to return to the main menu.");
    int quantity = GetNumberInput("Enter quantity. Type 0 to return to the main menu.");

    Console.Clear();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        using (var tableCommand = connection.CreateCommand())
        {
            tableCommand.CommandText =
                $"INSERT INTO records(date, quantity, habitId) VALUES ('{date}', {quantity}, {habitId})";

            tableCommand.ExecuteNonQuery();

        }

        connection.Close();
    }
}

void GetRecords()
{
    List<RecordWithHabit> records = new();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        using (var tableCommand = connection.CreateCommand())
        {
            tableCommand.CommandText = @"
                    SELECT records.Id, records.Date, records.Quantity, records.HabitId, habits.Name AS HabitName, habits.MeasurementUnit
                    FROM records
                    INNER JOIN habits ON records.HabitId = habits.Id";

            using (SqliteDataReader reader = tableCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            records.Add(new RecordWithHabit(
                            reader.GetInt32(0),
                            DateTime.ParseExact(reader.GetString(1), "MM-dd-yyyy", CultureInfo.InvariantCulture),
                            reader.GetInt32(2),
                            reader.GetString(4),
                            reader.GetString(5)));
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing record: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                }
            }
        }
    }

    ViewRecords(records);
}

void ViewRecords(List<RecordWithHabit> records)
{
    var table = new Table();
    table.AddColumn("Id");
    table.AddColumn("Date");
    table.AddColumn("Quantity");
    table.AddColumn("Habit Name");

    foreach (var record in records)
    {
        table.AddRow(record.Id.ToString(), record.Date.ToString(), $"{record.Quantity} {record.MeasurementUnit}", record.HabitName.ToString());
    }

    AnsiConsole.Write(table);
}

void DeleteRecord()
{
    GetRecords();

    var id = GetNumberInput("Enter the ID of the record you want to delete. Type 0 to return to the main menu.");

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        using (var tableCommand = connection.CreateCommand())
        {
            tableCommand.CommandText =
                $"DELETE FROM records WHERE Id = {id}";

            int rowsAffected = tableCommand.ExecuteNonQuery();
            if (rowsAffected != 0)
            {
                Console.WriteLine($"Record with ID {id} deleted successfully.");
            }
        }

        connection.Close();
    }
}

void UpdateRecord()
{
    GetRecords();

    var id = GetNumberInput("Enter the ID of the record you want to update. Type 0 to return to the main menu.");

    string date = "";
    bool updateDate = AnsiConsole.Confirm("Do you want to update the date?");
    if (updateDate)
    {
        date = GetDateInput("Enter the new date. (Format: mm-dd-yyyy). Type 0 to return to the main menu.");
    }

    int quantity = 0;
    bool updateQuantity = AnsiConsole.Confirm("Do you want to update the quantity?");
    if (updateQuantity)
    {
        quantity = GetNumberInput("Enter the new quantity. Type 0 to return to the main menu.");
    }

    string query;
    if (updateDate && updateQuantity)
    {
        query = $"UPDATE records SET Date = '{date}', Quantity = {quantity} WHERE Id = {id}";
    }
    else if (updateDate && !updateQuantity)
    {
        query = $"UPDATE records SET Date = '{date}' WHERE Id = {id}";
    }
    else
    {
        query = $"UPDATE records SET Quantity = {quantity} WHERE Id = {id}";
    }

    using (var connection = new SqliteConnection(connectionString))
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

string GetDateInput(string message)
{
    Console.WriteLine(message);
    string dateInput = Console.ReadLine();

    if (dateInput == "0") MainMenu();

    while (!DateTime.TryParseExact(dateInput, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
    {
        Console.WriteLine("Invalid date format. Please enter the date in mm-dd-yyyy format.");
        dateInput = Console.ReadLine();
    }

    return dateInput;
}

int GetNumberInput(string message)
{
    Console.WriteLine(message);
    string numberInput = Console.ReadLine();

    if (numberInput == "0") MainMenu();

    int output = 0;
    while (!int.TryParse(numberInput, out output) || output < 0)
    {
        Console.WriteLine("Invalid number. Try again");
        numberInput = Console.ReadLine();
    }

    return output;
}

record Habit(int Id, string Name, string UnitOfMeasurement);

record RecordWithHabit(int Id, DateTime Date, int Quantity, string HabitName, string MeasurementUnit);