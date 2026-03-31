using large_fantasy_model.Models.CharacterModels.Additional.Creature;
using System.ComponentModel.DataAnnotations;

namespace large_fantasy_model.Models.CharacterModels.Json
{
    public class Race
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string SubType { get; set; }

        public string Size { get; set; } // Change to enum

        public List<Feature> Traits { get; set; }

        public List<Action> Actions { get; set; }

        public Senses Senses { get; set; }
    }
}
