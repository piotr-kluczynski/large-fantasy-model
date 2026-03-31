namespace large_fantasy_model.Models.Character.References
{
    public class CharacterDamageType
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
        public int DamageTypeId { get; set; }
        public DamageCategory Category { get; set; }
    }

    public enum DamageCategory
    {
        Immunity,
        Resistance,
        Vulnerability
    }
}
