using System.Data;

namespace HabitLogger.Data;

public class HabitLoggerDbContext
{
    internal readonly IDbConnection ConnectionString;

    public HabitLoggerDbContext(IDbConnection connectionString)
    {
        ConnectionString = connectionString;
    }

    internal void CreateDatabase() { }

    private void SeedData() { }

    private bool IsTableEmpty(string tableName)
    {
        return false;
    }
}
