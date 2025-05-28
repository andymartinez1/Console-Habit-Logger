using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

var connectionString = @"Data Source=habbitlogger.db";

CreateDatabase();

MainMenu();

void CreateDatabase()
{
    using (SqliteConnection connection = new(connectionString))
    {
        using (SqliteCommand tableCmd = connection.CreateCommand())
        {
            connection.Open();
            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS daily_pushups (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER
                    )";
            tableCmd.ExecuteNonQuery();
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
                .Title("What would you like to do?")
                .AddChoices(
               "Add Record",
               "Delete Record",
               "View Records",
               "Update Record",
               "Quit")
               );

        switch (usersChoice)
        {
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

void InsertRecord()
{
    string date = GetDateInput("Enter the date. (Format: mm-dd-yyyy). Type 0 to return to the main menu.");

    int quantity = GetNumberInput("Enter quantity. Type 0 to return to the main menu.");

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        using (var createTableCommand = connection.CreateCommand())
        {
            createTableCommand.CommandText =
                $"INSERT INTO daily_pushups(date, quantity) VALUES ('{date}', {quantity})";

            createTableCommand.ExecuteNonQuery();

        }

        connection.Close();
    }
}

void GetRecords()
{
    List<PushupRecord> records = new();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        using (var createTableCommand = connection.CreateCommand())
        {
            createTableCommand.CommandText = "SELECT * FROM daily_pushups ORDER BY Date DESC";

            using (SqliteDataReader reader = createTableCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            records.Add(new PushupRecord(
                                reader.GetInt32(0),
                                DateTime.ParseExact(reader.GetString(1), "MM-dd-yyyy", CultureInfo.InvariantCulture),
                                reader.GetInt32(2)));
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

void ViewRecords(List<PushupRecord> records)
{
    var table = new Table();
    table.AddColumn("Id");
    table.AddColumn("Date");
    table.AddColumn("Quantity");

    foreach (var record in records)
    {
        table.AddRow(record.Id.ToString(), record.Date.ToString("MM-dd-yyyy"), record.Quantity.ToString());
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

        using (var createTableCommand = connection.CreateCommand())
        {
            createTableCommand.CommandText =
                $"DELETE FROM daily_pushups WHERE Id = {id}";

            int rowsAffected = createTableCommand.ExecuteNonQuery();
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
        query = $"UPDATE daily_pushups SET Date = '{date}', Quantity = {quantity} WHERE Id = {id}";
    }
    else if (updateDate && !updateQuantity)
    {
        query = $"UPDATE daily_pushups SET Date = '{date}' WHERE Id = {id}";
    }
    else 
    {
        query = $"UPDATE daily_pushups SET Quantity = {quantity} WHERE Id = {id}";
    }

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        using (var createTableCommand = connection.CreateCommand())
        {
            createTableCommand.CommandText = query;

            createTableCommand.ExecuteNonQuery();
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

record PushupRecord(int Id, DateTime Date, int Quantity)
{
    public override string ToString()
    {
        return $"{Id} | {Date:MM-dd-yyyy} | {Quantity}";
    }
}