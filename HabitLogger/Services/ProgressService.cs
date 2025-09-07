using HabitLogger.Data;
using HabitLogger.Utils;
using Spectre.Console;

namespace HabitLogger.Services;

public class ProgressService : IProgressService
{
    private readonly HabitLoggerDbContext _habitLoggerDbContext;
    private readonly IHabitService _habitService;

    public ProgressService(IHabitService habitService, HabitLoggerDbContext habitLoggerDbContext)
    {
        _habitService = habitService;
        _habitLoggerDbContext = habitLoggerDbContext;
    }

    public void InsertProgress()
    {
        var date = Helpers.GetDateInput(
            "Enter the date. (Format: mm-dd-yyyy). Type 0 to return to the main menu."
        );

        _habitService.GetHabits();

        var habitId = Helpers.GetNumberInput(
            "Enter the ID of the habit for which you want to add a record. Type 0 to return to the main menu."
        );
        var quantity = Helpers.GetNumberInput("Enter quantity. Type 0 to return to the main menu.");

        Console.Clear();
    }

    public void GetProgress() { }

    public void DeleteProgress() { }

    public void UpdateProgress()
    {
        GetProgress();

        var id = Helpers.GetNumberInput(
            "Enter the ID of the record you want to update. Type 0 to return to the main menu."
        );

        var date = "";
        var updateDate = AnsiConsole.Confirm("Do you want to update the date?");
        if (updateDate)
            date = Helpers.GetDateInput(
                "Enter the new date. (Format: mm-dd-yyyy). Type 0 to return to the main menu."
            );

        var quantity = 0;
        var updateQuantity = AnsiConsole.Confirm("Do you want to update the quantity?");
        if (updateQuantity)
            quantity = Helpers.GetNumberInput(
                "Enter the new quantity. Type 0 to return to the main menu."
            );
    }
}
