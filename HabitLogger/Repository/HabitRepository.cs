using Dapper;
using HabitLogger.Data;
using HabitLogger.Models;

namespace HabitLogger.Repository;

public class HabitRepository : IHabitRepository
{
    private readonly DatabaseContext _databaseContext;

    public HabitRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public void InsertHabit(Habit habit)
    {
        using (var connection = _databaseContext.ConnectionString)
        {
            connection.Open();

            var insertHabitQuery =
                $"INSERT INTO habits(Name, MeasurementUnit) VALUES ('{habit.Name}', '{habit.UnitOfMeasurement}')";

            connection.Execute(insertHabitQuery);

            connection.Close();
        }
    }

    public List<Habit> GetHabits()
    {
        using (var connection = _databaseContext.ConnectionString)
        {
            connection.Open();

            var getHabitsQuery = $"SELECT * FROM habits";

            var habits = connection.Query<Habit>(getHabitsQuery).ToList();

            return habits;
        }
    }

    public void UpdateHabit(Habit habit)
    {
        using (var connection = _databaseContext.ConnectionString)
        {
            connection.Open();

            var updateHabitQuery =
                $@"UPDATE habits 
                SET Name = '{habit.Name}', MeasurementUnit = '{habit.UnitOfMeasurement}' 
                WHERE Id = {habit.Id}";

            connection.Execute(updateHabitQuery);

            connection.Close();
        }
    }

    public void DeleteHabit(int id)
    {
        using (var connection = _databaseContext.ConnectionString)
        {
            connection.Open();

            var deleteHabitQuery = $"DELETE FROM habits WHERE Id = {id}";

            connection.Execute(deleteHabitQuery);

            connection.Close();
        }
    }
}
