using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.ViewModels
{
    public class CreateGameViewModel
    {
        [Required(ErrorMessage = "Name of the game is required.")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 25 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campaign description is required.")]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 2000 characters.")]
        public string Description { get; set; }
    }
}