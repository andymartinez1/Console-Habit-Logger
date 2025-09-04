namespace HabitLogger.Models;

public class Habit
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string UnitOfMeasurement { get; set; } = string.Empty;
}
