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
        public bool IsPublic { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Password must be at least 3 characters long.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please specify the maximum number of players.")]
        [Range(2, 50, ErrorMessage = "The campaign must have between 2 and 50 players.")]
        public int MaxPlayers { get; set; } = 10; 
    }
}