using large_fantasy_model.ViewModels.Compendium;

namespace large_fantasy_model.Models.Compendium
{
    /// <summary>
    /// Model opisujący różne "kategorie" systemu RPG.
    /// Każda z kategorii stanowi fundamentalną kategorię elemenentów opisujących system 
    /// - np. w systemie DnD każde ze stworzeń definiowane jest między innymi przez jego gatunek, 
    /// w takiej sytuacji gatunek jest kategorią tego systemu, a poszczególne gatunki, jego elementami.
    /// </summary>
    public class RulebookCategory
    {
        /// <summary>
        /// Identyfikator kategorii (np. 1). 
        /// Zbędne?
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tekstowe ID kategorii (np. "spells").
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Oficjalna nazwa kategorii wyświetlana dla użytkownika (np. "Spells").
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Nazwa folderu, który zawiera pliki JSON definiujące elementy systemu w aplikacji (np. "DnD_BasicRules_2018").
        /// </summary>
        public string FilesPathName { get; set; }

        /// <summary>
        /// Lista obiektów RulebookItemViewModel służąca do wstępnej reprezentacji elementów tej kategorii w widoku Details.
        /// </summary>
        public List<RulebookItemViewModel> Items { get; set; } = new();
    }
}
