using HabitLogger.Data;
using HabitLogger.Models;

namespace HabitLogger.Repository;

public class HabitRepository : IHabitRepository
{
    private readonly HabitLoggerDbContext _habitLoggerDbContext;

    public HabitRepository(HabitLoggerDbContext habitLoggerDbContext)
    {
        _habitLoggerDbContext = habitLoggerDbContext;
    }

    public void InsertHabit(Habit habit) { }

    public List<Habit> GetHabits()
    {
        return null;
    }

    public void UpdateHabit(Habit habit) { }

    public void DeleteHabit(int id) { }
}
