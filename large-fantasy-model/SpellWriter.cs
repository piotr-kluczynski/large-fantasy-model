using large_fantasy_model.Models.CharacterModels.Json;
using System.Text.Json;

namespace large_fantasy_model
{
    public class SpellWriter
    {
        private readonly string _path;

        public SpellWriter(IWebHostEnvironment env)
        {
            _path = Path.Combine(env.ContentRootPath, "Data", "CharacterJsonFiles", "Spells");
        }

        public void Save(Spell spell)
        {
            var fileName = GenerateFileName(spell.Name);

            var fullPath = Path.Combine(_path, fileName);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(spell, options);

            File.WriteAllText(fullPath, json);
        }

        private string GenerateFileName(string name)
        {
            return name
                .Trim()
                .ToLower()
                .Replace(" ", "_")
                + ".json";
        }
    }
}
