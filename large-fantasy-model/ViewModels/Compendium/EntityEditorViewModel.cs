namespace large_fantasy_model.ViewModels.Compendium
{
    /// <summary>
    /// Uniwersalny model służący do przechowywania informacji na temat edytowanego elementu w widoku EnityEditor.
    /// </summary>
    public class EntityEditorViewModel
    {
        /// <summary>
        /// Zmienna służąca do nawigacji w systemie plików (na wypadek zapisywania/nadpisywania elementu).
        /// </summary>
        public string Rulebook { get; set; }

        /// <summary>
        /// Zmienna służąca do nawigacji w systemie plików (na wypadek zapisywania/nadpisywania elementu).
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Nazwa elementu wyświetlana dla użytkownika.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Zmienna sygnalizująca czy tworzony jest nowy element, czy edytowany istniejący element.
        /// </summary>
        public bool IsEdit { get; set; }

        /// <summary>
        /// Lista modeli EntityFieldDefinition, które opisują kolejne pola elementu.
        /// </summary>
        public List<EntityFieldDefinition> Fields { get; set; }

        /// <summary>
        /// Słownik przechowujący nazwy oraz wartości pól elementu.
        /// </summary>
        public Dictionary<string, string?> Values { get; set; } = new();
    }
}
