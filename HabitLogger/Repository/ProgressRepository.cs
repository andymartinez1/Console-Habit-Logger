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

    public void InsertProgress(Progress progress) { }

    public List<Progress> GetProgress()
    {
        return null;
    }

    public void UpdateProgress(Progress progress) { }

    public void DeleteProgress(int id) { }
}
