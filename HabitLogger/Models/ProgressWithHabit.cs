namespace HabitLogger.Models;

public class ProgressWithHabit
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }

    public string HabitName { get; set; }

    public string MeasurementUnit { get; set; }
}
