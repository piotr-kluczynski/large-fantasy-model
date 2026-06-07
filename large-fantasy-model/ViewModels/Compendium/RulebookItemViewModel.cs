namespace large_fantasy_model.ViewModels.Compendium
{
    /// <summary>
    /// Uniwersalny model służący do reprezentacji dowolnego elementu z systemu RPG w widoku Details.
    /// Ma za zadanie umożliwić użytkownikowi rozróżnienie go spośród pozostałych elementów, 
    /// a systemowi pozwolić na odnalezienie poprawnego modelu opisującego w pełni element.
    /// </summary>
    public class RulebookItemViewModel
    {
        /// <summary>
        /// Oficjana nazwa elementu, przeznaczona dla użytkownika.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Krótki opis elementu, przeznaczony dla użytkownika.
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// Nazwa podręcznika (systemu RPG), do którego należy ten element. Pozwala systemowi na jego lokalizację w katalogu plików.
        /// </summary>
        public string Rulebook { get; set; }

        /// <summary>
        /// Nazwa "kategorii" systemu RPG, do którego należy ten element. Pozwala systemowi na jego lokalizację w katalogu plików.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Nazwa "robocza" elementu, wykorzystywana w nawigacji przez system. 
        /// </summary>
        public string Slug { get; set; }
    }
}
