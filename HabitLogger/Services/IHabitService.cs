using HabitLogger.Models;

namespace HabitLogger.Services;

public interface IHabitService
{
    public void InsertHabit();

    public void GetHabits();

    public Habit GetHabitById();

    public void UpdateHabit();

    public void DeleteHabit();
}
