using large_fantasy_model.Models.CharacterModels.Json;

namespace large_fantasy_model.ViewModels
{
    public class CreateCharacterViewModel
    {
        public List<CClass> AvailableClasses { get; set; }
        public List<Race> AvailableRaces { get; set; }
        public List<Background> AvailableBackgrounds { get; set; }
        public List<Item> AvailableItems { get; set; }
        public List<Weapon> AvailableWeapons { get; set; }
        public List<Spell> AvailableSpells { get; set; }

        public bool IsSpellcaster { get; set; }

        public string Name { get; set; }
        public int Level { get; set; }
        public string SelectedClassName { get; set; }
        public string SelectedRaceName { get; set; }
        public string SelectedBackgroundName { get; set; }
        public List<string> SelectedItemNames { get; set; }
        public List<string> SelectedWeaponNames { get; set; }
        public List<string> SelectedSpellNames { get; set; }

        // Abilities
        public int Strength { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Constitution { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int Wisdom { get; set; } = 10;
        public int Charisma { get; set; } = 10;

        // Saving Throws
        public bool StrengthSave { get; set; }
        public bool DexteritySave { get; set; }
        public bool ConstitutionSave { get; set; }
        public bool IntelligenceSave { get; set; }
        public bool WisdomSave { get; set; }
        public bool CharismaSave { get; set; }

        // Skills
        public int Athletics { get; set; }
        public int Acrobatics { get; set; }
        public int SleightOfHand { get; set; }
        public int Stealth { get; set; }
        public int Arcana { get; set; }
        public int History { get; set; }
        public int Investigation { get; set; }
        public int Nature { get; set; }
        public int Religion { get; set; }
        public int AnimalHandling { get; set; }
        public int Insight { get; set; }
        public int Medicine { get; set; }
        public int Perception { get; set; }
        public int Survival { get; set; }
        public int Deception { get; set; }
        public int Intimidation { get; set; }
        public int Performance { get; set; }
        public int Persuasion { get; set; }

        // Creature properties
        public string Alignment { get; set; }
        public int Speed { get; set; }
        public int MaxHitPoints { get; set; }
        public int TempHitPoints { get; set; }
        public int Inspiration { get; set; }
        public int ArmorClass { get; set; }
        public List<string> Languages { get; set; }

        // Character Details
        public int Age { get; set; }
        public string Eyes { get; set; }
        public string Hair { get; set; }
        public string Skin { get; set; }
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
