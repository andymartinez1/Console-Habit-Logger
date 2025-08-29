using System.ComponentModel.DataAnnotations;

namespace HabitLogger.Enums;

public enum MainMenuOptions
{
    [Display(Name = "Manage Habits")]
    ManageHabits,

    [Display(Name = "Manage Habit Progress")]
    ManageProgress,

    Quit,
}
