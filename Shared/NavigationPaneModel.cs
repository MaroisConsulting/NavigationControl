namespace Shared
{
    /// <summary>
    /// Models the data required for a Navigation Pane
    /// </summary>
    public class NavigationPaneModel : _EntityBase
    {
        /// <summary>
        /// The header of the section
        /// </summary>
        public string? Header { get; set; }

        /// <summary>
        /// The type of navigation items to load
        /// </summary>
        public NavigationItemType NavigationItemType { get; set; }

        /// <summary>
        /// Determines if the navigation pane is expanded
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// A delegate that represents the source of the data
        /// </summary>
        public Func<List<NavigationEntity>>? DataSource { get; set; }
    }
}
