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

    public void InsertProgress(Progress progress)
    {
        _habitLoggerDbContext.ProgressList.Add(progress);

        _habitLoggerDbContext.SaveChanges();
    }

    public List<Progress> GetProgress()
    {
        return _habitLoggerDbContext.ProgressList.ToList();
    }

    public Progress GetProgressById(int id)
    {
        return _habitLoggerDbContext.ProgressList.Find(id);
    }

    public void UpdateProgress(Progress progress)
    {
        _habitLoggerDbContext.ProgressList.Update(progress);

        _habitLoggerDbContext.SaveChanges();
    }

    public void DeleteProgress(int id)
    {
        var progress = GetProgressById(id);

        _habitLoggerDbContext.Remove(progress);
    }
}
