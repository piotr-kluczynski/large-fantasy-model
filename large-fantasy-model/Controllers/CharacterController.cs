using large_fantasy_model.Data;
using large_fantasy_model.Models.CharacterModels;
using large_fantasy_model.Models.CharacterModels.Additional;
using large_fantasy_model.Models.CharacterModels.Json;
using large_fantasy_model.Models.CharacterModels.References;
using large_fantasy_model.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace large_fantasy_model.Controllers
{
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

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet]
        public IActionResult Collection()
        {
            return View("Collection");
        }

        [HttpGet]
        public IActionResult Details()
        {
            return View("Details");
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
            var character = BuildCharacter(model);

            _context.Characters.Add(character);
            _context.SaveChanges();

            return RedirectToAction(
                nameof(Details),
                new { id = character.Id });
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View("Editor");
        }
        [HttpPost]
        public IActionResult Edit(EditCharacterViewModel model)
        {
            return View("Details");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            return View("Index");
        }

        private Character BuildCharacter(CreateCharacterViewModel model)
        {
            // Creating feature list
            List<string> feature_names = new List<string>();
        
            // Collecting features from class
            List<CClass> classes = _classRepository.GetAll(rulebook, "Classes");
            CClass selectedClass = classes.FirstOrDefault(c =>
                    c.Name.ToLower() == model.SelectedClassName.ToLower().Replace(" ", "_"));

            foreach(string feature in selectedClass.Features)
            {
                feature_names.Add(feature);
            }

            // Collecting features from Race
            List<Race> races = _raceRepository.GetAll(rulebook, "Races");
            Race selectedRace = races.FirstOrDefault(c =>
                    c.Name.ToLower() == model.SelectedRaceName.ToLower().Replace(" ", "_"));

            foreach (string feature in selectedRace.Features)
            {
                feature_names.Add(feature);
            }

            // Collecting features from Background
            List<Background> backgrounds = _backgroundRepository.GetAll(rulebook, "Backgrounds");
            Background selectedBackground = backgrounds.FirstOrDefault(c =>
                    c.Name.ToLower() == model.SelectedBackgroundName.ToLower().Replace(" ", "_"));

            foreach (string feature in selectedBackground.Features)
            {
                feature_names.Add(feature);
            }

            // Creating equipment dictionary
            var equipment_dict = model.SelectedItemNames
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());


            var character = new Character
            {
                UserId = 0,// Add current user id
                Name = model.Name,
                Xp = 0,
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
                    Eyes = model.Eyes,
                    Hair = model.Hair,
                    Skin = model.Skin,
                    Weight = model.Weight,
                    Height = model.Height,
                    Personality = model.Personality,
                    Ideal = model.Ideal,
                    Bond = model.Bond,
                    Flaw = model.Flaw,
                    Backstory = model.Backstory,
                    Physical = model.Physical,
                },
                Features = feature_names
                    .Select(feature => new CharacterFeature
                    {
                        FeatureName = feature
                    })
                    .ToList(),
                Spells = model.SelectedSpellNames
                    .Select(spell => new CharacterSpell
                    {
                        SpellName = spell
                    }).ToList(),
                Weapons = model.SelectedWeaponNames
                    .Select(weapon => new CharacterWeapon
                    {
                        WeaponName = weapon
                    }).ToList(),
                Equipment = equipment_dict
                    .Select(equipment => new CharacterEquipment
                    {
                        ItemName = equipment.Key,
                        Quantity = equipment.Value
                    }).ToList(),
                Alignment = model.Alignment,
                Speed = model.Speed,
                CurrentHitPoints = model.MaxHitPoints,
                MaxHitPoints = model.MaxHitPoints,
                Inspiration = model.Inspiration,
                ArmorClass = model.ArmorClass,
                Languages = model.Languages,
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
