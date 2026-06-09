using large_fantasy_model.Data;
using large_fantasy_model.Models;
using large_fantasy_model.Models.CharacterModels;
using large_fantasy_model.Models.CharacterModels.Additional;
using large_fantasy_model.Models.CharacterModels.Json;
using large_fantasy_model.Models.CharacterModels.References;
using large_fantasy_model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace large_fantasy_model.Controllers
{
    [Authorize]
    public class CharacterController : Controller
    {
        private readonly LargeFantasyModelContext _context;

        private readonly JsonRepository<Spell> _spellRepository;
        private readonly JsonRepository<Item> _itemRepository;
        private readonly JsonRepository<CClass> _classRepository;
        private readonly JsonRepository<Race> _raceRepository;
        private readonly JsonRepository<Weapon> _weaponRepository;
        private readonly JsonRepository<Background> _backgroundRepository;

        string rulebook = "DnD_BasicRules_2018";

        public CharacterController(
            LargeFantasyModelContext context,
            JsonRepository<Spell> spellRepository, 
            JsonRepository<Item> itemRepository, 
            JsonRepository<CClass> classRepository, 
            JsonRepository<Race> raceRepository, 
            JsonRepository<Weapon> weaponRepository,
            JsonRepository<Background> backgroundRepository)
        {
            _context = context;

            _spellRepository = spellRepository;
            _itemRepository = itemRepository;
            _classRepository = classRepository;
            _raceRepository = raceRepository;
            _weaponRepository = weaponRepository;
            _backgroundRepository = backgroundRepository;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Collection));
        }

        [HttpGet]
        public IActionResult Collection()
        {
            int myId = GetCurrentUserId();

            var characters = _context.Characters
                .Include(c => c.Class)
                .Include(c => c.Race)
                .Include(c => c.Background)
                .Where(c => c.UserId == myId)
                .ToList();
            return View("Collection", characters);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var character = _context.Characters
                .Include(c => c.Class)
                .Include(c => c.Race)
                .Include(c => c.Background)
                .Include(c => c.Details)
                .Include(c => c.AbilityScores)
                .Include(c => c.SavingThrows)
                .Include(c => c.Skills)
                .Include(c => c.Features)
                .Include(c => c.Spells)
                .Include(c => c.Weapons)
                .Include(c => c.Equipment)
                .FirstOrDefault(c => c.Id == id);

            if (character == null) return NotFound();

            return View("Details", character);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateCharacterViewModel
            {
                AvailableClasses = _classRepository.GetAll(rulebook, "Classes"),
                AvailableRaces = _raceRepository.GetAll(rulebook, "Races"),
                AvailableBackgrounds = _backgroundRepository.GetAll(rulebook, "Backgrounds"),
                AvailableItems = _itemRepository.GetAll(rulebook, "Items"),
                AvailableWeapons = _weaponRepository.GetAll(rulebook, "Weapons"),
                AvailableSpells = _spellRepository.GetAll(rulebook, "Spells")
            };
            return View("Creator", model);
        }
        [HttpPost]
        public IActionResult Create(CreateCharacterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableClasses = _classRepository.GetAll(rulebook, "Classes");
                model.AvailableRaces = _raceRepository.GetAll(rulebook, "Races");
                model.AvailableBackgrounds = _backgroundRepository.GetAll(rulebook, "Backgrounds");
                model.AvailableItems = _itemRepository.GetAll(rulebook, "Items");
                model.AvailableWeapons = _weaponRepository.GetAll(rulebook, "Weapons");
                model.AvailableSpells = _spellRepository.GetAll(rulebook, "Spells");
                return View("Creator", model);
            }

            var character = BuildCharacter(model);

            _context.Characters.Add(character);
            _context.SaveChanges();

            return RedirectToAction(
                nameof(Details),
                new { id = character.Id });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var character = _context.Characters
                .Include(c => c.Class)
                .Include(c => c.Race)
                .Include(c => c.Background)
                .Include(c => c.Details)
                .Include(c => c.AbilityScores)
                .Include(c => c.SavingThrows)
                .Include(c => c.Skills)
                .Include(c => c.Features)
                .Include(c => c.Spells)
                .Include(c => c.Weapons)
                .Include(c => c.Equipment)
                .FirstOrDefault(c => c.Id == id);

            if (character == null) return NotFound();

            var model = new EditCharacterViewModel
            {
                Id = character.Id,
                Name = character.Name,
                Level = character.Level,
                Xp = character.Xp,
                Alignment = character.Alignment,
                Speed = character.Speed,
                MaxHitPoints = character.MaxHitPoints,
                Inspiration = character.Inspiration,
                ArmorClass = character.ArmorClass,
                Languages = character.Languages,

                SelectedClassName = character.Class?.ClassName,
                SelectedRaceName = character.Race?.RaceName,
                SelectedBackgroundName = character.Background?.BackgroundName,

                SelectedSpellNames = character.Spells?.Select(s => s.SpellName).ToList() ?? new List<string>(),
                SelectedWeaponNames = character.Weapons?.Select(w => w.WeaponName).ToList() ?? new List<string>(),
                SelectedItemNames = character.Equipment?.SelectMany(e => Enumerable.Repeat(e.ItemName, e.Quantity)).ToList() ?? new List<string>(),

                // Abilities
                Strength = character.AbilityScores?.Strength ?? 10,
                Dexterity = character.AbilityScores?.Dexterity ?? 10,
                Constitution = character.AbilityScores?.Constitution ?? 10,
                Intelligence = character.AbilityScores?.Intelligence ?? 10,
                Wisdom = character.AbilityScores?.Wisdom ?? 10,
                Charisma = character.AbilityScores?.Charisma ?? 10,

                // Saves
                StrengthSave = character.SavingThrows?.Strength ?? false,
                DexteritySave = character.SavingThrows?.Dexterity ?? false,
                ConstitutionSave = character.SavingThrows?.Constitution ?? false,
                IntelligenceSave = character.SavingThrows?.Intelligence ?? false,
                WisdomSave = character.SavingThrows?.Wisdom ?? false,
                CharismaSave = character.SavingThrows?.Charisma ?? false,

                // Skills
                Athletics = character.Skills?.Athletics ?? 0,
                Acrobatics = character.Skills?.Acrobatics ?? 0,
                SleightOfHand = character.Skills?.SleightOfHand ?? 0,
                Stealth = character.Skills?.Stealth ?? 0,
                Arcana = character.Skills?.Arcana ?? 0,
                History = character.Skills?.History ?? 0,
                Investigation = character.Skills?.Investigation ?? 0,
                Nature = character.Skills?.Nature ?? 0,
                Religion = character.Skills?.Religion ?? 0,
                AnimalHandling = character.Skills?.AnimalHandling ?? 0,
                Insight = character.Skills?.Insight ?? 0,
                Medicine = character.Skills?.Medicine ?? 0,
                Perception = character.Skills?.Perception ?? 0,
                Survival = character.Skills?.Survival ?? 0,
                Deception = character.Skills?.Deception ?? 0,
                Intimidation = character.Skills?.Intimidation ?? 0,
                Performance = character.Skills?.Performance ?? 0,
                Persuasion = character.Skills?.Persuasion ?? 0,

                // Details
                Age = character.Details?.Age ?? 0,
                Eyes = character.Details?.Eyes,
                Hair = character.Details?.Hair,
                Skin = character.Details?.Skin,
                Weight = character.Details?.Weight ?? 0,
                Height = character.Details?.Height,
                Personality = character.Details?.Personality,
                Ideal = character.Details?.Ideal,
                Bond = character.Details?.Bond,
                Flaw = character.Details?.Flaw,
                Backstory = character.Details?.Backstory,
                Physical = character.Details?.Physical,

                AvailableClasses = _classRepository.GetAll(rulebook, "Classes"),
                AvailableRaces = _raceRepository.GetAll(rulebook, "Races"),
                AvailableBackgrounds = _backgroundRepository.GetAll(rulebook, "Backgrounds"),
                AvailableItems = _itemRepository.GetAll(rulebook, "Items"),
                AvailableWeapons = _weaponRepository.GetAll(rulebook, "Weapons"),
                AvailableSpells = _spellRepository.GetAll(rulebook, "Spells")
            };

            // Calculate IsSpellcaster based on class
            var cClass = model.AvailableClasses.FirstOrDefault(c => c.Name.ToLower().Replace(" ", "_") == model.SelectedClassName?.ToLower());
            model.IsSpellcaster = cClass != null && !string.IsNullOrWhiteSpace(cClass.Spellcasting);

            return View("Editor", model);
        }
        
        [HttpPost]
        public IActionResult Edit(EditCharacterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableClasses = _classRepository.GetAll(rulebook, "Classes");
                model.AvailableRaces = _raceRepository.GetAll(rulebook, "Races");
                model.AvailableBackgrounds = _backgroundRepository.GetAll(rulebook, "Backgrounds");
                model.AvailableItems = _itemRepository.GetAll(rulebook, "Items");
                model.AvailableWeapons = _weaponRepository.GetAll(rulebook, "Weapons");
                model.AvailableSpells = _spellRepository.GetAll(rulebook, "Spells");
                return View("Editor", model);
            }

            var existingCharacter = _context.Characters
                .Include(c => c.Class)
                .Include(c => c.Race)
                .Include(c => c.Background)
                .Include(c => c.Details)
                .Include(c => c.AbilityScores)
                .Include(c => c.SavingThrows)
                .Include(c => c.Skills)
                .Include(c => c.Features)
                .Include(c => c.Spells)
                .Include(c => c.Weapons)
                .Include(c => c.Equipment)
                .FirstOrDefault(c => c.Id == model.Id);

            if (existingCharacter == null) return NotFound();

            var newCharacterData = BuildCharacter(model);

            existingCharacter.Name = newCharacterData.Name;
            existingCharacter.Level = newCharacterData.Level;
            existingCharacter.Xp = newCharacterData.Xp;
            existingCharacter.Alignment = newCharacterData.Alignment;
            existingCharacter.Speed = newCharacterData.Speed;
            existingCharacter.MaxHitPoints = newCharacterData.MaxHitPoints;
            existingCharacter.CurrentHitPoints = newCharacterData.MaxHitPoints; // Reset HP on edit
            existingCharacter.Inspiration = newCharacterData.Inspiration;
            existingCharacter.ArmorClass = newCharacterData.ArmorClass;
            existingCharacter.Languages = newCharacterData.Languages;

            if (existingCharacter.Class != null && newCharacterData.Class != null) 
                existingCharacter.Class.ClassName = newCharacterData.Class.ClassName;
            else existingCharacter.Class = newCharacterData.Class;

            if (existingCharacter.Race != null && newCharacterData.Race != null) 
                existingCharacter.Race.RaceName = newCharacterData.Race.RaceName;
            else existingCharacter.Race = newCharacterData.Race;

            if (existingCharacter.Background != null && newCharacterData.Background != null) 
                existingCharacter.Background.BackgroundName = newCharacterData.Background.BackgroundName;
            else existingCharacter.Background = newCharacterData.Background;

            if (existingCharacter.Details != null) _context.Entry(existingCharacter.Details).CurrentValues.SetValues(newCharacterData.Details);
            else existingCharacter.Details = newCharacterData.Details;

            if (existingCharacter.AbilityScores != null) _context.Entry(existingCharacter.AbilityScores).CurrentValues.SetValues(newCharacterData.AbilityScores);
            else existingCharacter.AbilityScores = newCharacterData.AbilityScores;

            if (existingCharacter.SavingThrows != null) _context.Entry(existingCharacter.SavingThrows).CurrentValues.SetValues(newCharacterData.SavingThrows);
            else existingCharacter.SavingThrows = newCharacterData.SavingThrows;

            if (existingCharacter.Skills != null) _context.Entry(existingCharacter.Skills).CurrentValues.SetValues(newCharacterData.Skills);
            else existingCharacter.Skills = newCharacterData.Skills;

            _context.Features.RemoveRange(existingCharacter.Features);
            existingCharacter.Features = newCharacterData.Features;

            _context.Spells.RemoveRange(existingCharacter.Spells);
            existingCharacter.Spells = newCharacterData.Spells;

            _context.Weapons.RemoveRange(existingCharacter.Weapons);
            existingCharacter.Weapons = newCharacterData.Weapons;

            _context.Equipment.RemoveRange(existingCharacter.Equipment);
            existingCharacter.Equipment = newCharacterData.Equipment;

            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = existingCharacter.Id });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            int myId = GetCurrentUserId();
            var character = _context.Characters.FirstOrDefault(c => c.Id == id && c.UserId == myId);
            if (character != null)
            {
                _context.Characters.Remove(character);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Collection));
        }

        private Character BuildCharacter(CreateCharacterViewModel model)
        {
            // Creating feature list
            List<string> feature_names = new List<string>();
        
            // Collecting features from class
            List<CClass> classes = _classRepository.GetAll(rulebook, "Classes");
            CClass selectedClass = classes.FirstOrDefault(c =>
                    c.Name.ToLower() == model.SelectedClassName.ToLower());
            
            if (selectedClass != null && selectedClass.Features != null)
            {
                foreach(string feature in selectedClass.Features)
                {
                    feature_names.Add(feature);
                }
            }

            // Collecting features from Race
            List<Race> races = _raceRepository.GetAll(rulebook, "Races");
            Race selectedRace = races.FirstOrDefault(c =>
                    c.Name.ToLower() == model.SelectedRaceName.ToLower());

            if (selectedRace != null && selectedRace.Features != null)
            {
                foreach (string feature in selectedRace.Features)
                {
                    feature_names.Add(feature);
                }
            }

            // Collecting features from Background
            List<Background> backgrounds = _backgroundRepository.GetAll(rulebook, "Backgrounds");
            Background selectedBackground = backgrounds.FirstOrDefault(c =>
                    c.Name.ToLower() == model.SelectedBackgroundName?.ToLower());

            if (selectedBackground != null && selectedBackground.Features != null)
            {
                foreach (string feature in selectedBackground.Features)
                {
                    feature_names.Add(feature);
                }
            }

            // Creating equipment dictionary - CORRECT
            var equipment_dict = model.SelectedItemNames != null 
                ? model.SelectedItemNames.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count()) 
                : new Dictionary<string, int>();

            int myId = GetCurrentUserId();

            var character = new Character
            {
                UserId = myId,
                Name = model.Name,
                Xp = model.Xp,
                Level = model.Level,
                Race = new CharacterRace
                {
                    RaceName = model.SelectedRaceName.ToLower().Replace(" ", "_")
                },
                Class = new CharacterClass
                {
                    ClassName = model.SelectedClassName.ToLower().Replace(" ", "_")
                },
                Background = new CharacterBackground
                {
                    BackgroundName = model.SelectedBackgroundName.ToLower().Replace(" ", "_")
                },
                Details = new CharacterDetails
                {
                    Age = model.Age,
                    Eyes = model.Eyes ?? "",
                    Hair = model.Hair ?? "",
                    Skin = model.Skin ?? "",
                    Weight = model.Weight,
                    Height = model.Height ?? "",
                    Personality = model.Personality ?? "",
                    Ideal = model.Ideal ?? "",
                    Bond = model.Bond ?? "",
                    Flaw = model.Flaw ?? "",
                    Backstory = model.Backstory ?? "",
                    Physical = model.Physical ?? "",
                },
                Features = feature_names
                    .Select(feature => new CharacterFeature
                    {
                        FeatureName = feature
                    })
                    .ToList(),
                Spells = model.SelectedSpellNames != null ? model.SelectedSpellNames
                    .Select(spell => new CharacterSpell
                    {
                        SpellName = spell.ToLower().Replace(" ", "_")
                    }).ToList() : new List<CharacterSpell>(),
                Weapons = model.SelectedWeaponNames != null ? model.SelectedWeaponNames
                    .Select(weapon => new CharacterWeapon
                    {
                        WeaponName = weapon.ToLower().Replace(" ", "_")
                    }).ToList() : new List<CharacterWeapon>(),
                Equipment = equipment_dict
                    .Select(equipment => new CharacterEquipment
                    {
                        ItemName = equipment.Key.ToLower().Replace(" ", "_"),
                        Quantity = equipment.Value
                    }).ToList(),
                Alignment = model.Alignment ?? "",
                Speed = model.Speed,
                CurrentHitPoints = model.MaxHitPoints,
                MaxHitPoints = model.MaxHitPoints,
                Inspiration = model.Inspiration,
                ArmorClass = model.ArmorClass,
                Languages = model.Languages ?? "",
                AbilityScores = new CharacterAbilityScores
                {
                    Strength = model.Strength,
                    Dexterity = model.Dexterity,
                    Constitution = model.Constitution,
                    Intelligence = model.Intelligence,
                    Wisdom = model.Wisdom,
                    Charisma = model.Charisma
                },
                SavingThrows = new CharacterSavingThrows
                {
                    Strength = model.StrengthSave,
                    Dexterity = model.DexteritySave,
                    Constitution = model.ConstitutionSave,
                    Intelligence = model.IntelligenceSave,
                    Wisdom = model.WisdomSave,
                    Charisma = model.CharismaSave
                },
                Skills = new CharacterSkills
                {
                    Athletics = model.Athletics,
                    Acrobatics = model.Acrobatics,
                    SleightOfHand = model.SleightOfHand,
                    Stealth = model.Stealth,
                    Arcana = model.Arcana,
                    History = model.History,
                    Investigation = model.Investigation,
                    Nature = model.Nature,
                    Religion = model.Religion,
                    AnimalHandling = model.AnimalHandling,
                    Insight = model.Insight,
                    Medicine = model.Medicine,
                    Perception = model.Perception,
                    Survival = model.Survival,
                    Deception = model.Deception,
                    Intimidation = model.Intimidation,
                    Performance = model.Performance,
                    Persuasion = model.Persuasion
                }
            };

            return character;
        }
    }
}
