using System.ComponentModel.DataAnnotations;

namespace HabitLogger.Enums;

public enum MenuOptions
{
    [Display(Name = "Add Habit")]
    AddHabit,

    [Display(Name = "Delete Habit")]
    DeleteHabit,

    [Display(Name = "Update Habit")]
    UpdateHabit,

    [Display(Name = "Add Progress")]
    AddProgress,

    [Display(Name = "Delete Progress")]
    DeleteProgress,

    [Display(Name = "View All Progress")]
    ViewAllProgress,

    [Display(Name = "Update Progress")]
    UpdateProgress,
    Quit,
}
