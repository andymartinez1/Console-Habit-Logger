using HabitLogger.Models;

namespace HabitLogger.Repository;

public interface IProgressRepository
{
    public void InsertProgress(HabitProgress habitProgress);

    public List<HabitProgress> GetProgressList();

    public HabitProgress GetProgressById(int id);

    public void UpdateProgress(HabitProgress habitProgress);

    public void DeleteProgress(int id);
}
