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
        /// Słownik przechowujący nazwy oraz wartości pól elementu (dla typów text/number/textarea/checkbox).
        /// </summary>
        public Dictionary<string, string?> Values { get; set; } = new();

        /// <summary>
        /// Dostępne opcje dla pól typu "entity-list" (Key pola → lista opcji do wyświetlenia jako checkboxy).
        /// Wypełniany przez kontroler przed wyświetleniem formularza.
        /// </summary>
        public Dictionary<string, List<EntitySelectOption>> AvailableOptions { get; set; } = new();

        /// <summary>
        /// Aktualnie zaznaczone wartości pól typu "entity-list" (Key pola → lista wybranych wartości).
        /// Wypełniany przez widok i odbierany przez kontroler przy POST.
        /// </summary>
        public Dictionary<string, List<string>> ListValues { get; set; } = new();
    }

    /// <summary>
    /// Pojedyncza opcja dostępna do zaznaczenia w polu typu "entity-list".
    /// </summary>
    public class EntitySelectOption
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }
}
