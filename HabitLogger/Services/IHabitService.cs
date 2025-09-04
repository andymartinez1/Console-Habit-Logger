namespace HabitLogger.Services;

public interface IHabitService
{
    public static void InsertHabit();

    public static void GetHabits();

    public static void ViewAllHabits(List<Data.Data.Habit> habits);

    public static void DeleteHabit();

    public static void UpdateHabit();

    public static void InsertProgress();

    public static void GetProgress();

    public static void ViewAllProgress(List<Data.Data.ProgressWithHabit> progress);

    public static void DeleteProgress();

    public static void UpdateProgress();

    public static string GetDateInput(string message);

    public static int GetNumberInput(string message);
}
