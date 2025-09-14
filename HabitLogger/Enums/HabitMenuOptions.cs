using System.ComponentModel.DataAnnotations;

namespace HabitLogger.Enums;

public enum HabitMenuOptions
{
    [Display(Name = "Add Habit")]
    AddHabit,

    [Display(Name = "View All Habits")]
    ViewAllHabits,

    [Display(Name = "Delete Habit")]
    DeleteHabit,

    [Display(Name = "Update Habit")]
    UpdateHabit,

    [Display(Name = "Back to Main Menu")]
    BackToMainMenu,
}
