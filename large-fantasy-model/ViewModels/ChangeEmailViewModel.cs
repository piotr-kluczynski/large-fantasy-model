using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.ViewModels
{
    public class ChangeEmailViewModel
    {
        [Required(ErrorMessage = "New Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string NewEmail { get; set; }

        [Required(ErrorMessage = "Current password is required to verify your identity.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
    }
}