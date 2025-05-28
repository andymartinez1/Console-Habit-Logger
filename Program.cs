using Microsoft.Data.Sqlite;

var connectionString = "Data Source=habbitlogger.db";

CreateDatabase();

void CreateDatabase()
{
    using (SqliteConnection connection = new (connectionString))
    {

        // Create a table if it doesn't exist
        using (SqliteCommand createTableCommand = connection.CreateCommand())
        {
            connection.Open();

            createTableCommand.CommandText =
                @"CREATE TABLE IF NOT EXISTS habits (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER,
                )";
            
            createTableCommand.ExecuteNonQuery();
        }
    }
}

static void GetUserInput()
{
    Console.Clear();
    bool closeApp = false;
    while (!closeApp)
    {
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("0. Close Application");
        Console.WriteLine("1. View All Habits");
        Console.WriteLine("2. Create New Habit");
        Console.WriteLine("3. Delete Habit");
        Console.WriteLine("4. Update Habit");
        Console.WriteLine("-------------------------------------------\n");

        string userInput = Console.ReadLine();

        switch (userInput)
        {
            case "0":
                Console.WriteLine("\nGoodbye.");
                closeApp = true;
                break;
            /*case "1":
                GetAllRecords();
                break;*/
            /*case "2":
                InsertRecord();
                break;*/
            /*case "3":
                DeleteRecord();
                break;
            case "4":
                UpdateRecord();
                break;*/
            default:
                Console.WriteLine("\nInvalid input. Please type a number between 0 and 4.");
                break;
        }
    }
}

/*static void InsertRecord()
{
    string date = GetDateInput();

    int quantity = GetNumberInput("Quantity:");
    
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        // Create a table if it doesn't exist
        var createTableCommand = connection.CreateCommand();

        createTableCommand.CommandText =
            $"INSERT INTO habits(date, quantity) VALUES('{date}', {quantity})";

        createTableCommand.ExecuteNonQuery();

        connection.Close();
    }
}*/

static string GetDateInput()
{
    Console.WriteLine("Enter the date: (Format: mm/dd/yyyy). Type 0 to return to the main menu.");
    string dateInput = Console.ReadLine();

    if (dateInput == "0") GetUserInput();

    return dateInput;
}

static int GetNumberInput(string message)
{
    Console.WriteLine(message);
    string numberInput = Console.ReadLine();
    
    if (numberInput == "0") GetUserInput();
    
    int finalInput = Convert.ToInt32(numberInput);
    
    return finalInput;
}