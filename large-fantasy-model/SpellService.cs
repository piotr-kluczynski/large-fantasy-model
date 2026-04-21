using large_fantasy_model.Models.CharacterModels.Json;
using System.Text.Json;

namespace large_fantasy_model
{
    public class SpellService
    {
        private readonly string _spellPath;

        public SpellService(IWebHostEnvironment env)
        {
            _spellPath = Path.Combine(env.ContentRootPath, "Data", "CharacterJsonFiles", "Spells");
        }

        public List<Spell> GetAllSpells()
        {
            var spells = new List<Spell>();

            var files = Directory.GetFiles(_spellPath, "*.json");

            foreach (var file in files)
            {
                var json = System.IO.File.ReadAllText(file);
                var spell = JsonSerializer.Deserialize<Spell>(json);

                if (spell != null)
                {
                    spells.Add(spell);
                }
            }

            return spells;
        }
    }
}
