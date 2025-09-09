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

    public void InsertHabit(Habit habit)
    {
        _habitLoggerDbContext.Add(habit);

        _habitLoggerDbContext.SaveChanges();
    }

    public List<Habit> GetHabits()
    {
        return _habitLoggerDbContext.Habits.ToList();
    }

    public Habit GetHabitById(int id)
    {
        return _habitLoggerDbContext.Habits.Find(id);
    }

    public void UpdateHabit(Habit habit)
    {
        _habitLoggerDbContext.Habits.Update(habit);

        _habitLoggerDbContext.SaveChanges();
    }

    public void DeleteHabit(int id)
    {
        var habit = GetHabitById(id);

        _habitLoggerDbContext.Habits.Remove(habit);
    }
}
