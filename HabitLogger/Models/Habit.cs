namespace HabitLogger.Models;

public class Habit
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<Progress> ProgressList { get; set; } = new List<Progress>();
}