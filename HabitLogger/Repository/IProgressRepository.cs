using HabitLogger.Models;

namespace HabitLogger.Repository;

public interface IProgressRepository
{
    public void InsertProgress(Progress progress);

    public List<Progress> GetProgress();

    public void UpdateProgress(Progress progress);

    public void DeleteProgress(int id);
}
