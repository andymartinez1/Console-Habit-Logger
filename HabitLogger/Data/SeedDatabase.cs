using HabitLogger.Models;

namespace HabitLogger.Data;

public static class SeedDatabase
{
    public static void SeedData(HabitLoggerDbContext context)
    {
        var Habits = new List<Habit>
        {
            new() { Name = "Drinking Water", UnitOfMeasurement = "Cups" },
            new() { Name = "Reading", UnitOfMeasurement = "Pages" },
            new() { Name = "Walking", UnitOfMeasurement = "Miles" },
        };
        var Progress = new List<Progress>
        {
            new()
            {
                Quantity = 5,
                Date = new DateTime(2025, 9, 15, 12, 0, 0),
                HabitId = 1,
            },
            new()
            {
                Quantity = 11,
                Date = new DateTime(2025, 9, 12, 19, 30, 0),
                HabitId = 2,
            },
            new()
            {
                Quantity = 4,
                Date = new DateTime(2025, 9, 13, 11, 0, 0),
                HabitId = 3,
            },
        };

        context.Habits.AddRange(Habits);
        context.ProgressList.AddRange(Progress);
        context.SaveChanges();
    }
}
