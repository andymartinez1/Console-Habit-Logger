using System.ComponentModel.DataAnnotations;

namespace HabitLogger.Enums;

public enum ProgressMenuOptions
{
    [Display(Name = "Add Progress")]
    AddProgress,

    [Display(Name = "View All Progress")]
    ViewAllProgress,

    [Display(Name = "View Progress by ID")]
    ViewProgressById,

    [Display(Name = "Update Progress")]
    UpdateProgress,

    [Display(Name = "Delete Progress")]
    DeleteProgress,

    [Display(Name = "Back to Main Menu")]
    BackToMainMenu,
}
