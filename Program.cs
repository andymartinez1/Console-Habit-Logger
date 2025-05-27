using Microsoft.Data.Sqlite;

string connectionString = "Data Source=habbitlogger.db";

using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();

    // Create a table if it doesn't exist
    var createTableCommand = connection.CreateCommand();

    createTableCommand.CommandText = 
        @"CREATE TABLE IF NOT EXISTS habits (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Description TEXT,
            Quantity INTEGER,
            Date TEXT NOT NULL
            )";

    createTableCommand.ExecuteNonQuery();

    connection.Close();
}