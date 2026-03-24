namespace large_fantasy_model.Models.Character
{
    // Class describing the character background in the database
    public class CharacterBackground
    {
        public int Id { get; set; }
        public int Character {  get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
