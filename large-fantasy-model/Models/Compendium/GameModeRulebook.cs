using large_fantasy_model.Models.CharacterModels.Json;

namespace large_fantasy_model.Models.Compendium
{
    public class GameModeRulebook
    {
        public int Id { get; set; }
     
        public string Title { get; set; }

        public string Description { get; set; }

        public string IconEmoji { get; set; }

        public string PdfFileName { get; set; }

        public string FilesPathName { get; set; }

        public List<string> Overview { get; set; } = new();

        public List<RulebookCategory> Categories { get; set; } = new();
    }
}
