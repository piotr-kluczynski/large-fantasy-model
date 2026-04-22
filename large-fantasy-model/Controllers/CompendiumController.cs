using Microsoft.AspNetCore.Mvc;
using large_fantasy_model.Models;
using large_fantasy_model.Models.CharacterModels.Json;
using large_fantasy_model.ViewModels;

namespace large_fantasy_model.Controllers
{
    public class CompendiumController : Controller
    {
        private readonly JsonRepository<Spell> _spellRepository;

        public CompendiumController(JsonRepository<Spell> spellRepository)
        {
            _spellRepository = spellRepository;
        }

        [HttpGet]
        public IActionResult CreateSpell(string rulebook, string category)
        {
            var model = new CreateSpellViewModel
            {
                Rulebook = rulebook,
                Category = category
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateSpell(CreateSpellViewModel model)
        {
            var spell = new Spell
            {
                Name = model.Name,
                Description = model.Description,
                HigherLevel = model.HigherLevel,
                Level = model.Level,
                CastingTime = model.CastingTime,
                RangeArea = model.RangeArea,
                Components = model.Components,
                Material = model.Material,
                Ritual = model.Ritual,
                Concetration = model.Concentration,
                Duration = model.Duration,
                School = model.School,
                AttackSave = model.AttackSave,
                DamageEffect = model.DamageEffect
            };

            _spellRepository.Save(spell, model.Rulebook, model.Category);

            return RedirectToAction("Details", new { id = 1 });
        }

        public IActionResult Index()
        {
            return View(GetRulebooks());
        }

        public IActionResult Details(int id)
        {
            var rulebook = GetRulebooks()
                .FirstOrDefault(x => x.Id == id);

            if (rulebook == null)
                return NotFound();

            foreach (var category in rulebook.Categories)
            {
                if (category.Key == "spells")
                {
                    var items = _spellRepository.GetAll(
                        rulebook.FilesPathName,
                        category.FilesPathName);

                    category.Items = items.Cast<IRulebookEntity>().ToList();
                }
            }

            return View(rulebook);
        }

        public IActionResult Spell(string rulebook, string category, string name)
        {
            var spells = _spellRepository.GetAll(rulebook, category);
            var normalized = name.Replace("-", " ");

            var spell = spells.FirstOrDefault(s =>
                s.Name.ToLower() == normalized.ToLower());

            if (spell == null)
            {
                return NotFound();
            }

            return View(spell);
        }

        private List<GameModeRulebook> GetRulebooks()
        {
            return new List<GameModeRulebook>
            {
                new GameModeRulebook
                {
                    Id = 1,
                    Title = "DnD Basic Rules 2018",
                    Description = "D&D Basic Rules, Version 1.0, Released November 2018",
                    IconEmoji = "DnD_BasicRules_2018.png",
                    PdfFileName = "DnD_BasicRules_2018.pdf",
                    FilesPathName = "DnD_BasicRules_2018",
                    Overview = [
                        "<h3>Introduction</h3>",
                        "The Dungeons & Dragons roleplaying game is about\r\nstorytelling in worlds of swords and sorcery. It shares elements with childhood games of make-believe. Like those\r\ngames, D&D is driven by imagination. It’s about picturing\r\nthe towering castle beneath the stormy night sky and\r\nimagining how a fantasy adventurer might react to the\r\nchallenges that scene presents.Unlike a game of make-believe, D&D gives structure to\r\nthe stories, a way of determining the consequences of the\r\nadventurers’ action. Players roll dice to resolve whether\r\ntheir attacks hit or miss or whether their adventurers\r\ncan scale a cliff, roll away from the strike of a magical\r\nlightning bolt, or pull off some other dangerous task. Anything is possible, but the dice make some outcomes more\r\nprobable than others. In the Dungeons & Dragons game, each player creates an adventurer (also called a character) and teams\r\nup with other adventurers (played by friends). Working together, the group might explore a dark dungeon, a ruined city, a haunted castle, a lost temple deep in a jungle,\r\nor a lava-filled cavern beneath a mysterious mountain.\r\nThe adventurers can solve puzzles, talk with other characters, battle fantastic monsters, and discover fabulous\r\nmagic items and other treasure. One player, however, takes on the role of the Dungeon\r\nMaster (DM), the game’s lead storyteller and referee. The\r\nDM creates adventures for the characters, who navigate\r\nits hazards and decide which paths to explore. The DM\r\nmight describe the entrance to Castle Ravenloft, and the\r\nplayers decide what they want their adventurers to do.\r\nWill they walk across the dangerously weathered drawbridge? Tie themselves together with rope to minimize\r\nthe chance that someone will fall if the drawbridge gives\r\nway? Or cast a spell to carry them over the chasm?\r\nThen the DM determines the results of the adventurers’\r\nactions and narrates what they experience. Because the\r\nDM can improvise to react to anything the players attempt, D&D is infinitely flexible, and each adventure can\r\nbe exciting and unexpected.\r\nThe game has no real end; when one story or quest\r\nwraps up, another one can begin, creating an ongoing\r\nstory called a campaign. Many people who play the\r\ngame keep their campaigns going for months or years,\r\nmeeting with their friends every week or so to pick up\r\nthe story where they left off. The adventurers grow in\r\nmight as the campaign continues. Each monster defeated, each adventure completed, and each treasure\r\nrecovered not only adds to the continuing story, but also\r\nearns the adventurers new capabilities. This increase\r\nin power is reflected by an adventurer’s level.\r\nThere’s no winning and losing in the Dungeons &\r\nDragons game—at least, not the way those terms are\r\nusually understood. Together, the DM and the players\r\ncreate an exciting story of bold adventurers who confront\r\ndeadly perils. Sometimes an adventurer might come to\r\na grisly end, torn apart by ferocious monsters or done in\r\nby a nefarious villain. Even so, the other adventurers can\r\nsearch for powerful magic to revive their fallen comrade,\r\nor the player might choose to create a new character to\r\ncarry on. The group might fail to complete an adventure\r\nsuccessfully, but if everyone had a good time and created a\r\nmemorable story, they all win.",
                        "<h3>Worlds of Adventure</h3>",
                        "The many worlds of the Dungeons & Dragons game\r\nare places of magic and monsters, of brave warriors and\r\nspectacular adventures. They begin with a foundation of\r\nmedieval fantasy and then add the creatures, places, and\r\nmagic that make these worlds unique.\r\nThe worlds of the Dungeons & Dragons game exist\r\nwithin a vast cosmos called the multiverse, connected\r\nin strange and mysterious ways to one another and to\r\nother planes of existence, such as the Elemental Plane\r\nof Fire and the Infinite Depths of the Abyss. Within this\r\nmultiverse are an endless variety of worlds. Many of\r\nthem have been published as official settings for the D&D\r\ngame. The legends of the Forgotten Realms, Dragonlance, Greyhawk, Dark Sun, Mystara, and Eberron settings are\r\nwoven together in the fabric of the multiverse. Alongside\r\nthese worlds are hundreds of thousands more, created\r\nby generations of D&D players for their own games. And\r\namid all the richness of the multiverse, you might create a\r\nworld of your own.\r\nAll these worlds share characteristics, but each world\r\nis set apart by its own history and cultures, distinctive\r\nmonsters and races, fantastic geography, ancient dungeons, and scheming villains. Some races have unusual\r\ntraits in different worlds. The halflings of the Dark Sun\r\nsetting, for example, are jungle-dwelling cannibals, and\r\nthe elves are desert nomads. Some worlds feature races\r\nunknown in other settings, such as Eberron’s warforged,\r\nsoldiers created and imbued with life to fight in the Last\r\nWar. Some worlds are dominated by one great story, like\r\nthe War of the Lance that plays a central role in the Dragonlance setting. But they’re all D&D worlds, and you can\r\nuse the rules in this book to create a character and play\r\nin any one of them.\r\nYour DM might set the campaign on one of these\r\nworlds or on one that he or she created. Because there is\r\nso much diversity among the worlds of D&D, you should\r\ncheck with your DM about any house rules that will affect\r\nyour play of the game. Ultimately, the Dungeon Master is\r\nthe authority on the campaign and its setting, even if the\r\nsetting is a published world.",
                        "<h3>How to Play</h3>",
                        "The play of the Dungeons & Dragons game unfolds according to this basic pattern.\r\n1. The DM describes the environment. The DM\r\ntells the players where their adventurers are and what’s\r\naround them, presenting the basic scope of options that\r\npresent themselves (how many doors lead out of a room,\r\nwhat’s on a table, who’s in the tavern, and so on).\r\n2. The players describe what they want to do. Sometimes one player speaks for the whole party, saying,\r\n“We’ll take the east door,” for example. Other times,\r\ndifferent adventurers do different things: one adventurer\r\nmight search a treasure chest while a second examines\r\nan esoteric symbol engraved on a wall and a third keeps\r\nwatch for monsters. The players don’t need to take turns,\r\nbut the DM listens to every player and decides how to resolve those actions.\r\nSometimes, resolving a task is easy. If an adventurer\r\nwants to walk across a room and open a door, the DM\r\nmight just say that the door opens and describe what lies\r\nbeyond. But the door might be locked, the floor might\r\nhide a deadly trap, or some other circumstance might\r\nmake it challenging for an adventurer to complete a task.\r\nIn those cases, the DM decides what happens, often\r\nrelying on the roll of a die to determine the results of\r\nan action.\r\n3. The DM narrates the results of the adventurers’\r\nactions. Describing the results often leads to another decision point, which brings the flow of the game right back\r\nto step 1.\r\nThis pattern holds whether the adventurers are cautiously exploring a ruin, talking to a devious prince, or\r\nlocked in mortal combat against a mighty dragon. In\r\ncertain situations, particularly combat, the action is more\r\nstructured and the players (and DM) do take turns choosing and resolving actions. But most of the time, play is\r\nfluid and flexible, adapting to the circumstances of the\r\nadventure.\r\nOften the action of an adventure takes place in the\r\nimagination of the players and DM, relying on the DM’s\r\nverbal descriptions to set the scene. Some DMs like to\r\nuse music, art, or recorded sound effects to help set the\r\nmood, and many players and DMs alike adopt different\r\nvoices for the various adventurers, monsters, and other\r\ncharacters they play in the game. Sometimes, a DM\r\nmight lay out a map and use tokens or miniature figures\r\nto represent each creature involved in a scene to help the\r\nplayers keep track of where everyone is.",
                        "<h4>Game Dice</h4>",
                        "The game uses polyhedral dice with different numbers of\r\nsides. You can find dice like these in game stores and in\r\nmany bookstores.\r\nIn these rules, the different dice are referred to by the\r\nletter d followed by the number of sides: d4, d6, d8, d10,\r\nd12, and d20. For instance, a d6 is a six-sided die (the\r\ntypical cube that many games use).\r\nPercentile dice, or d100, work a little differently. You\r\ngenerate a number between 1 and 100 by rolling two\r\ndifferent ten-sided dice numbered from 0 to 9. One die\r\n(designated before you roll) gives the tens digit, and\r\nthe other gives the ones digit. If you roll a 7 and a 1, for example, the number rolled is 71. Two 0s represent 100.\r\nSome ten-sided dice are numbered in tens (00, 10, 20,\r\nand so on), making it easier to distinguish the tens digit\r\nfrom the ones digit. In this case, a roll of 70 and 1 is 71,\r\nand 00 and 0 is 100.\r\nWhen you need to roll dice, the rules tell you how many\r\ndice to roll of a certain type, as well as what modifiers to\r\nadd. For example, “3d8 + 5” means you roll three eightsided dice, add them together, and add 5 to the total.\r\nThe same d notation appears in the expressions “1d3”\r\nand “1d2.” To simulate the roll of 1d3, roll a d6 and divide\r\nthe number rolled by 2 (round up). To simulate the roll of\r\n1d2, roll any die and assign a 1 or 2 to the roll depending\r\non whether it was odd or even. (Alternatively, if the number rolled is more than half the number of sides on the\r\ndie, it’s a 2.",
                        "<h4>The D20</h4>",
                        "Does an adventurer’s sword swing hurt a dragon or just\r\nbounce off its iron-hard scales? Will the ogre believe an\r\noutrageous bluff? Can a character swim across a raging\r\nriver? Can a character avoid the main blast of a fireball,\r\nor does he or she take full damage from the blaze? In\r\ncases where the outcome of an action is uncertain, the\r\nDungeons & Dragons game relies on rolls of a 20-sided\r\ndie, a d20, to determine success or failure.\r\nEvery character and monster in the game has capabilities defined by six ability scores. The abilities are\r\nStrength, Dexterity, Constitution, Intelligence, Wisdom,\r\nand Charisma, and they typically range from 3 to 18 for\r\nmost adventurers. (Monsters might have scores as low as\r\n1 or as high as 30.) These ability scores, and the ability\r\nmodifiers derived from them, are the basis for almost\r\nevery d20 roll that a player makes on a character’s or\r\nmonster’s behalf.\r\nAbility checks, attack rolls, and saving throws are the\r\nthree main kinds of d20 rolls, forming the core of the\r\nrules of the game. All three follow these simple steps.\r\n1. Roll the die and add a modifier. Roll a d20 and add\r\nthe relevant modifier. This is typically the modifier derived from one of the six ability scores, and it sometimes\r\nincludes a proficiency bonus to reflect a character’s particular skill. (See chapter 1 for details on each ability and\r\nhow to determine an ability’s modifier.)\r\n2. Apply circumstantial bonuses and penalties. A\r\nclass feature, a spell, a particular circumstance, or some\r\nother effect might give a bonus or penalty to the check.\r\n3. Compare the total to a target number. If the total\r\nequals or exceeds the target number, the ability check,\r\nattack roll, or saving throw is a success. Otherwise, it’s a\r\nfailure. The DM is usually the one who determines target\r\nnumbers and tells players whether their ability checks,\r\nattack rolls, and saving throws succeed or fail.\r\nThe target number for an ability check or a saving\r\nthrow is called a Difficulty Class (DC). The target number for an attack roll is called an Armor Class (AC).\r\nThis simple rule governs the resolution of most tasks\r\nin D&D play. Chapter 7 provides more detailed rules for\r\nusing the d20 in the game.",
                        "<h4>Advantage and Disadvantage</h4>",
                        "Sometimes an ability check, attack roll, or saving throw\r\nis modified by special situations called advantage and disadvantage. Advantage reflects the positive circumstances\r\nsurrounding a d20 roll, while disadvantage reflects the\r\nopposite. When you have either advantage or disadvantage, you roll a second d20 when you make the roll. Use\r\nthe higher of the two rolls if you have advantage, and use\r\nthe lower roll if you have disadvantage. For example, if\r\nyou have disadvantage and roll a 17 and a 5, you use the\r\n5. If you instead have advantage and roll those numbers,\r\nyou use the 17.\r\nMore detailed rules for advantage and disadvantage are\r\npresented in chapter 7.",
                        "<h4>Specific Beats General</h4>",
                        "This book contains rules, especially in parts 2 and 3, that\r\ngovernspecific r how the game plays. That said, many racial traits,\r\nclass features, spells, magic items, monster abilities, and\r\nother game elements break the general rules in some\r\nway, creating an exception to how the rest of the game\r\nworks. Remember this: If a rule contradicts a\r\ngeneral rule, the specific rule wins.\r\nExceptions to the rules are often minor. For instance,\r\nmany adventurers don’t have proficiency with longbows,\r\nbut every wood elf does because of a racial trait. That\r\ntrait creates a minor exception in the game. Other examples of rule-breaking are more conspicuous. For instance,\r\nan adventurer can’t normally pass through walls, but\r\nsome spells make that possible. Magic accounts for most\r\nof the major exceptions to the rules.",
                        "<h4></h4>",
                        "There’s one more general rule you need to know at the\r\noutset. Whenever you divide a number in the game, round\r\ndown if you end up with a fraction, even if the fraction is\r\none-half or greater.",
                        "<h3>Adventures</h3>",
                        "The Dungeons & Dragons game consists of a group of\r\ncharacters embarking on an adventure that the Dungeon\r\nMaster presents to them. Each character brings particular capabilities to the adventure in the form of ability\r\nscores and skills, class features, racial traits, equipment,\r\nand magic items. Every character is different, with various strengths and weaknesses, so the best party of\r\nadventurers is one in which the characters complement\r\neach other and cover the weaknesses of their companions. The adventurers must cooperate to successfully\r\ncomplete the adventure.\r\nThe adventure is the heart of the game, a story with\r\na beginning, a middle, and an end. An adventure might\r\nbe created by the Dungeon Master or purchased off the\r\nshelf, tweaked and modified to suit the DM’s needs and\r\ndesires. In either case, an adventure features a fantastic\r\nsetting, whether it’s an underground dungeon, a crumbling castle, a stretch of wilderness, or a bustling city. It\r\nfeatures a rich cast of characters: the adventurers created\r\nand played by the other players at the table, as well as\r\nnonplayer characters (NPCs). Those characters might\r\nbe patrons, allies, enemies, hirelings, or just background extras in an adventure. Often, one of the NPCs is a villain\r\nwhose agenda drives much of an adventure’s action.\r\nOver the course of their adventures, the characters are\r\nconfronted by a variety of creatures, objects, and situations that they must deal with in some way. Sometimes\r\nthe adventurers and other creatures do their best to\r\nkill or capture each other in combat. At other times, the\r\nadventurers talk to another creature (or even a magical\r\nobject) with a goal in mind. And often, the adventurers\r\nspend time trying to solve a puzzle, bypass an obstacle,\r\nfind something hidden, or unravel the current situation.\r\nMeanwhile, the adventurers explore the world, making\r\ndecisions about which way to travel and what they’ll try\r\nto do next.\r\nAdventures vary in length and complexity. A short adventure might present only a few challenges, and it might\r\ntake no more than a single game session to complete. A\r\nlong adventure can involve hundreds of combats, interactions, and other challenges, and take dozens of sessions\r\nto play through, stretching over weeks or months of real\r\ntime. Usually, the end of an adventure is marked by the\r\nadventurers heading back to civilization to rest and enjoy\r\nthe spoils of their labors.\r\nBut that’s not the end of the story. You can think of\r\nan adventure as a single episode of a TV series, made\r\nup of multiple exciting scenes. A campaign is the whole\r\nseries—a string of adventures joined together, with a consistent group of adventurers following the narrative from\r\nstart to finish.",
                        "<h4>The Three Pillars of Adventure</h4>",
                        "Adventurers can try to do anything their players can\r\nimagine, but it can be helpful to talk about their activities\r\nin three broad categories: exploration, social interaction,\r\nand combat.\r\nExploration includes both the adventurers’ movement\r\nthrough the world and their interaction with objects and\r\nsituations that require their attention. Exploration is the\r\ngive-and-take of the players describing what they want\r\ntheir characters to do, and the Dungeon Master telling\r\nthe players what happens as a result. On a large scale,\r\nthat might involve the characters spending a day crossing\r\na rolling plain or an hour making their way through caverns underground. On the smallest scale, it could mean\r\none character pulling a lever in a dungeon room to see\r\nwhat happens.\r\nSocial interaction features the adventurers talking to\r\nsomeone (or something) else. It might mean demanding\r\nthat a captured scout reveal the secret entrance to the\r\ngoblin lair, getting information from a rescued prisoner,\r\npleading for mercy from an orc chieftain, or persuading\r\na talkative magic mirror to show a distant location to the\r\nadventurers.\r\nThe rules in chapters 7 and 8 support exploration and\r\nsocial interaction, as do many class features in chapter 3\r\nand personality traits in chapter 4.\r\nCombat, the focus of chapter 9, involves characters\r\nand other creatures swinging weapons, casting spells,\r\nmaneuvering for position, and so on—all in an effort \r\nto defeat their opponents, whether that means killing\r\nevery enemy, taking captives, or forcing a rout. Combat\r\nis the most structured element of a D&D session, with\r\ncreatures taking turns to make sure that everyone gets\r\na chance to act. Even in the context of a pitched battle,\r\nthere’s still plenty of opportunity for adventurers to attempt wacky stunts like surfing down a flight of stairs on\r\na shield, to examine the environment (perhaps by pulling\r\na mysterious lever), and to interact with other creatures,\r\nincluding allies, enemies, and neutral parties.",
                        "<h4>The Wonders of Magic</h4>",
                        "Few D&D adventures end without something magical\r\nhappening. Whether helpful or harmful, magic appears\r\nfrequently in the life of an adventurer, and it is the focus\r\nof chapters 10 and 11.\r\nIn the worlds of Dungeons & Dragons, practitioners\r\nof magic are rare, set apart from the masses of people\r\nby their extraordinary talent. Common folk might see\r\nevidence of magic on a regular basis, but it’s usually\r\nminor—a fantastic monster, a visibly answered prayer,\r\na wizard walking through the streets with an animated\r\nshield guardian as a bodyguard.\r\nFor adventurers, though, magic is key to their survival.\r\nWithout the healing magic of clerics and paladins, adventurers would quickly succumb to their wounds. Without\r\nthe uplifting magical support of bards and clerics, warriors might be overwhelmed by powerful foes. Without\r\nthe sheer magical power and versatility of wizards and\r\ndruids, every threat would be magnified tenfold.\r\nMagic is also a favored tool of villains. Many adventures are driven by the machinations of spellcasters who\r\nare hellbent on using magic for some ill end. A cult leader\r\nseeks to awaken a god who slumbers beneath the sea, a\r\nhag kidnaps youths to magically drain them of their vigor,\r\na mad wizard labors to invest an army of automatons\r\nwith a facsimile of life, a dragon begins a mystical ritual\r\nto rise up as a god of destruction—these are just a few\r\nof the magical threats that adventurers might face. With\r\nmagic of their own, in the form of spells and magic items,\r\nthe adventurers might prevail!"
                        ],
                    Categories = new List<RulebookCategory>
                    {
                        new RulebookCategory
                        {
                            Id = 1,
                            Key = "spells",
                            Title = "Spells",
                            FilesPathName = "Spells"
                        }
                    }
                }
            };
        }
    }
}
