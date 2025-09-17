using HabitLogger.Models;

namespace HabitLogger.Services;

public interface IProgressService
{
    public void InsertProgress();

    public void GetProgress();

    public HabitProgress GetProgressById();

    public void UpdateProgress();

    public void DeleteProgress();
}
