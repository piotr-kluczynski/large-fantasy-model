namespace large_fantasy_model.Models.Compendium
{
    /// <summary>
    /// Uniwersalny interfejs łączący wszystkie elementy dowolnego systemu RPG. Zbędny?
    /// </summary>
    public interface IRulebookEntity
    {
        /// <summary>
        /// Tekstowe ID elementu.
        /// </summary>
        string Name { get; set; }
    }
}
