using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your username.")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Username should be between 3 and 25 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must meet all complexity requirements listed below the field.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // NOWE POLE: Potwierdzenie hasła (nie idzie do bazy danych, służy tylko walidacji)
        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}