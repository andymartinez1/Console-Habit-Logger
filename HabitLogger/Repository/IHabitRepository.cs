using HabitLogger.Models;

namespace HabitLogger.Repository;

public interface IHabitRepository
{
    public void InsertHabit(Habit habit);

    public List<Habit> GetHabits();

    public void UpdateHabit(Habit habit);

    public void DeleteHabit(int id);
}
