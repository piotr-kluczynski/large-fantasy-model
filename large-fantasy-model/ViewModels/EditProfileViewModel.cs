using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace large_fantasy_model.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [MaxLength(500, ErrorMessage = "Biography cannot exceed 500 characters.")]
        public string? Bio { get; set; }

        public IFormFile? ProfilePicture { get; set; }
        public string? CurrentProfilePicturePath { get; set; }
    }
}