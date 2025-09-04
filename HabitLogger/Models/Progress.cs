namespace HabitLogger.Models;

public class Progress
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }

    public string HabitName { get; set; } = string.Empty;

    public string UnitOfMeasurement { get; set; } = string.Empty;
}
