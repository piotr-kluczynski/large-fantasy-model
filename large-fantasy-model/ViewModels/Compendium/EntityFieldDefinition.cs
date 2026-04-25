namespace large_fantasy_model.ViewModels.Compendium
{
    /// <summary>
    /// Uniwersalny model służący do opisu pola danego elementu.
    /// Wykorzystywany przez widok EntityEditor, aby umożliwić użytkownikowi edytowanie właściwości elementów.
    /// </summary>
    public class EntityFieldDefinition
    {
        /// <summary>
        /// Tekstowe ID pola elementu.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Nazwa pola wyświetlana dla użytkownika
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Typ pola; obecnie obsługiwane są trzy różne typy pól:
        /// - "text"
        /// - "number"
        /// - "textarea"
        /// - "checkbox"
        /// </summary>
        public string Type { get; set; }

        // TO-DO
        public bool ShowInDetails { get; set; } = true;
    }
}
