using HabitLogger.Data;
using HabitLogger.Repository;
using HabitLogger.Utils;
using HabitLogger.Views;
using Spectre.Console;
using Progress = HabitLogger.Models.Progress;

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
        Progress progress = new Progress();

        _habitService.GetHabits();

        progress.HabitId = UserInputValidation.ValidateNumberInput(
            "Enter the ID of the habit for which you want to add a record."
        );

        var date = UserInputValidation.ValidateDateInput("Enter the date. (Format: mm-dd-yyyy).");

        progress.Date = DateTime.Parse(date);

        progress.Quantity = UserInputValidation.ValidateNumberInput("Enter quantity.");

        _progressRepository.InsertProgress(progress);

        AnsiConsole.MarkupLine("[Green]Progress created successfully![/]");
    }

    public void GetProgress()
    {
        var progress = _progressRepository.GetProgress();

        if (progress.Any())
            UserInterface.ViewAllProgress(progress);
        else
            AnsiConsole.MarkupLine(
                "[Red]No progress to display. Please add new habit progress.[/]"
            );
    }

    public Progress GetProgressById()
    {
        GetProgress();

        var id = UserInputValidation.ValidateNumberInput("Select the ID of the habit progress");

        return _progressRepository.GetProgressById(id);
    }

    public void UpdateProgress()
    {
        var progress = GetProgressById();

        progress.Id = UserInputValidation.ValidateNumberInput(
            "Enter the ID of the record you want to update. Type 0 to return to the main menu."
        );

        var updateDate = AnsiConsole.Confirm("Do you want to update the date?");
        if (updateDate)
        {
            var date = UserInputValidation.ValidateDateInput(
                "Enter the date. (Format: mm-dd-yyyy)."
            );

            progress.Date = DateTime.Parse(date);
        }

        var updateQuantity = AnsiConsole.Confirm("Do you want to update the quantity?");
        if (updateQuantity)
            progress.Quantity = UserInputValidation.ValidateNumberInput(
                "Enter the new quantity. Type 0 to return to the main menu."
            );

        _progressRepository.UpdateProgress(progress);

        AnsiConsole.MarkupLine("[Green]Progress updated successfully![/]");
    }

    public void DeleteProgress()
    {
        var progress = GetProgressById();

        if (!UserInputValidation.IsProgressValid(progress))
            return;

        var id = progress.Id;

        _progressRepository.DeleteProgress(id);

        AnsiConsole.MarkupLine("[Green]Progress deleted successfully![/]");
    }
}
