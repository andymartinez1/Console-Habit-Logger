using HabitLogger.Data;
using HabitLogger.Repository;
using HabitLogger.Utils;
using Spectre.Console;

namespace HabitLogger.Services;

public class ProgressService : IProgressService
{
    private readonly HabitLoggerDbContext _habitLoggerDbContext;
    private readonly IHabitService _habitService;
    private readonly IProgressRepository _progressRepository;

    public ProgressService(
        IHabitService habitService,
        HabitLoggerDbContext habitLoggerDbContext,
        IProgressRepository progressRepository
    )
    {
        _habitService = habitService;
        _habitLoggerDbContext = habitLoggerDbContext;
        _progressRepository = progressRepository;
    }

    public void InsertProgress()
    {
        var date = UserInputValidation.ValidateDateInput(
            "Enter the date. (Format: mm-dd-yyyy). Type 0 to return to the main menu."
        );

        _habitService.GetHabits();

        var habitId = UserInputValidation.ValidateNumberInput(
            "Enter the ID of the habit for which you want to add a record. Type 0 to return to the main menu."
        );
        var quantity = UserInputValidation.ValidateNumberInput(
            "Enter quantity. Type 0 to return to the main menu."
        );

        Console.Clear();
    }

    public void GetProgress() { }

    public void DeleteProgress() { }

    public void UpdateProgress()
    {
        GetProgress();

        var id = UserInputValidation.ValidateNumberInput(
            "Enter the ID of the record you want to update. Type 0 to return to the main menu."
        );

        var date = "";
        var updateDate = AnsiConsole.Confirm("Do you want to update the date?");
        if (updateDate)
            date = UserInputValidation.ValidateDateInput(
                "Enter the new date. (Format: mm-dd-yyyy). Type 0 to return to the main menu."
            );

        var quantity = 0;
        var updateQuantity = AnsiConsole.Confirm("Do you want to update the quantity?");
        if (updateQuantity)
            quantity = UserInputValidation.ValidateNumberInput(
                "Enter the new quantity. Type 0 to return to the main menu."
            );
    }
}
