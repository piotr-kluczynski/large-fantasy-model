namespace large_fantasy_model.Models.CharacterModels.Json
{
    public class Weapon : Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        
        public string Damage {  get; set; }

        public string VersatileDamage { get; set; }

        public string Range { get; set; }

        public string ThrowRange { get; set; }
    
        // Properties
        public string Ammunition { get; set; }
        public bool Finesse { get; set; }
        public bool Heavy { get; set; }
        public bool Light { get; set; }
        public bool Loading { get; set; }
        public bool Monk { get; set; }
        public bool Reach { get; set; }
        public bool Thrown { get; set; }
        public bool TwoHanded { get; set; }
        public bool Versatile { get; set; }
    }
}
