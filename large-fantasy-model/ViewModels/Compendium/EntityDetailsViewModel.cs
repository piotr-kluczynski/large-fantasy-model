namespace large_fantasy_model.ViewModels.Compendium
{
    /// <summary>
    /// Uniwersalny model przechowujący szczegółowe dane na temat elementu, wyświetlane dla użytkownika w widoku EntityDetails.
    /// </summary>
    public class EntityDetailsViewModel
    {

        /// <summary>
        /// Oficjalna nazwa elementu wyświetlana dla użytkownika.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Nazwa podręcznika (systemu RPG), do którego należy ten element. Pozwala systemowi na jego lokalizację w katalogu plików.
        /// </summary>
        public string Rulebook { get; set; }

        /// <summary>
        /// Nazwa "kategorii" systemu RPG, do którego należy ten element. Pozwala systemowi na jego lokalizację w katalogu plików.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Lista obiektów EntityFieldValue, które opisują pola elementów.
        /// </summary>

        public List<EntityFieldValue> Fields { get; set; } = new();

        /// <summary>
        /// Opcjonalne podlisty powiązanych elementów (np. lista Features dla Class/Race),
        /// renderowane jako sekcje z klikalnymi linkami w widoku EntityDetails.
        /// </summary>
        public List<EntitySubList> SubLists { get; set; } = new();
    }

    /// <summary>
    /// Prosta klasa służąca do tekstowej reprezentacji dowolnych właściwości elementów.
    /// </summary>
    public class EntityFieldValue
    {
        /// <summary>
        /// Nazwa pola.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Wartość pola.
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Sekcja z listą powiązanych elementów wyświetlana w widoku EntityDetails (np. "Features").
    /// </summary>
    public class EntitySubList
    {
        /// <summary>Nagłówek sekcji wyświetlany użytkownikowi.</summary>
        public string Label { get; set; }

        /// <summary>Pozycje listy — każda renderowana jako link do widoku Entity danego elementu.</summary>
        public List<EntitySubListItem> Items { get; set; } = new();
    }

    /// <summary>
    /// Pojedynczy element w podliście, przechowujący dane potrzebne do wygenerowania linku.
    /// </summary>
    public class EntitySubListItem
    {
        public string Name { get; set; }
        public string Rulebook { get; set; }
        public string Category { get; set; }
        public string Slug { get; set; }
    }
}
