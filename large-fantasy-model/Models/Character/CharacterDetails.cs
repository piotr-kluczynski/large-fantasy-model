using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models.Character
{
    // Class describing the character details in the database
    public class CharacterDetails
    {
        [Range(0, int.MaxValue)]
        public int Age { get; set; }
        public string Eyes { get; set; }
        public string Hair { get; set; }
        public string Skin { get; set; }
        [Range(0, int.MaxValue)]
        public int Weight { get; set; }
        public string Height { get; set; }
        public string Personality { get; set; }
        public string Ideal { get; set; }
        public string Bond { get; set; }
        public string Flaw { get; set; }
        public string Backstory { get; set; }
        public string Physical { get; set; }
    }
}
