using HabitLogger.Data;
using HabitLogger.Models;

namespace HabitLogger.Repository;

public class ProgressRepository : IProgressRepository
{
    private readonly HabitLoggerDbContext _habitLoggerDbContext;

    public ProgressRepository(HabitLoggerDbContext habitLoggerDbContext)
    {
        _habitLoggerDbContext = habitLoggerDbContext;
    }

    public void InsertProgress(HabitProgress habitProgress)
    {
        _habitLoggerDbContext.ProgressList.Add(habitProgress);

        _habitLoggerDbContext.SaveChanges();
    }

    public List<HabitProgress> GetProgressList()
    {
        return _habitLoggerDbContext.ProgressList.ToList();
    }

    public HabitProgress GetProgressById(int id)
    {
        return _habitLoggerDbContext.ProgressList.Find(id);
    }

    public void UpdateProgress(HabitProgress habitProgress)
    {
        _habitLoggerDbContext.ProgressList.Update(habitProgress);

        _habitLoggerDbContext.SaveChanges();
    }

    public void DeleteProgress(int id)
    {
        var progress = GetProgressById(id);

        _habitLoggerDbContext.Remove(progress);

        _habitLoggerDbContext.SaveChanges();
    }
}
