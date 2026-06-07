using large_fantasy_model.Models.Compendium;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace large_fantasy_model
{
    public class JsonRepository<T> where T : IRulebookEntity
    {
        private readonly string _basePath;

        public JsonRepository(string basePath)
        {
            _basePath = basePath;
        }

        public List<T> GetAll(string rulebook, string category)
        {
            var path = Path.Combine(_basePath, rulebook, category);

            return Directory.GetFiles(path, "*.json")
                .Select(file => JsonSerializer.Deserialize<T>(System.IO.File.ReadAllText(file)))
                .ToList();
        }

        public void Save(T entity, string rulebook, string category)
        {
            var fileName = Regex.Replace(entity.Name.ToLower().Replace(" ", "_"), @"[^\w\s]", "") + ".json";
            var path = Path.Combine(_basePath, rulebook, category, fileName);

            var json = JsonSerializer.Serialize(entity, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            System.IO.File.WriteAllText(path, json);
        }
    }
}
