using Dapper;
using HabitLogger.Data;
using HabitLogger.Models;

namespace HabitLogger.Repository;

public class ProgressRepository : IProgressRepository
{
    private readonly DatabaseContext _databaseContext;

    public ProgressRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public void InsertProgress(Progress progress)
    {
        using (var connection = _databaseContext.ConnectionString)
        {
            connection.Open();

            var insertProgressQuery =
                $"INSERT INTO progress(date, quantity, habitId) VALUES (@Date, @Quantity, @HabitId)";

            connection.Execute(
                insertProgressQuery,
                new
                {
                    progress.Date,
                    progress.Quantity,
                    progress.HabitName,
                }
            );

            connection.Close();
        }
    }

    public List<Progress> GetProgress()
    {
        using (var connection = _databaseContext.ConnectionString)
        {
            connection.Open();

            var getProgressQuery = $"SELECT * FROM progress";

            var progress = connection.Query<Progress>(getProgressQuery).ToList();

            return progress;
        }
    }

    public void UpdateProgress(Progress progress)
    {
        using (var connection = _databaseContext.ConnectionString)
        {
            connection.Open();

            var updateProgressQuery =
                @"UPDATE progress 
                SET Date = @Date, Quantity = @Quantity
                WHERE Id = @Id";

            connection.Execute(updateProgressQuery, new { progress.Date, progress.Quantity });

            connection.Close();
        }
    }

    public void DeleteProgress(int id)
    {
        using (var connection = _databaseContext.ConnectionString)
        {
            connection.Open();

            var deleteProgressQuery = $"DELETE FROM progress WHERE Id = {id}";

            connection.Execute(deleteProgressQuery);

            connection.Close();
        }
    }
}
