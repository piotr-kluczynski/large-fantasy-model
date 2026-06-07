using large_fantasy_model.Models.CharacterModels.Json;

namespace large_fantasy_model.Models.Compendium
{
    /// <summary>
    /// Model reprezentujący system RPG dostępny w aplikacji
    /// </summary>
    public class GameModeRulebook
    {
        public int Id { get; set; }
     
        /// <summary>
        /// Nazwa systemu (czasami podręcznika konkretnej edycji) (np. "DnD Basic Rules 2018")
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Krótki opis systemu (np. "D&D Basic Rules, Version 1.0, Released November 2018")
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Nazwa obrazu, który ma być wyświetlony aby reprezentować dany system np. ("DnD_BasicRules_2018.png")
        /// </summary>
        public string IconEmoji { get; set; }

        /// <summary>
        /// Nazwa pliku pdf (książki), który stanowił źródło dla tego systemu RPG (np. "DnD_BasicRules_2018.pdf")
        /// </summary>
        public string PdfFileName { get; set; }

        /// <summary>
        /// Nazwa folderu, który zawiera pliki JSON definiujące elementy systemu w aplikacji (np. "DnD_BasicRules_2018")
        /// </summary>
        public string FilesPathName { get; set; }

        /// <summary>
        /// Lista stringów, które stanowią paragrafy tekstu (HTML) stanowiące pełniejszy opis oraz podstawowe zasady rozgrywki w danym systemie.
        /// </summary>
        public List<string> Overview { get; set; } = new();

        /// <summary>
        /// Lista modeli RulebookCategory, które definiują tzw. "kategorie" systemu.
        /// Kategorie stanowią kluczowe i stałe elementy definiujące system RPG (np. możliwe klasy postaci, dostępne zaklęcia, itp.).
        /// </summary>
        public List<RulebookCategory> Categories { get; set; } = new();
    }
}
