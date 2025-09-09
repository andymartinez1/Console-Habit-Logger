namespace HabitLogger.Models;

public class Progress
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }

    public int HabitId { get; set; }

    public Habit Habit { get; set; }
}
