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
}
