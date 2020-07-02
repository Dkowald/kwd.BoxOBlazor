using System.Collections.Generic;

namespace ForServer.Pages.Shared.Shell
{
    /// <summary>
    /// Main UI model for presenting a tile-based layout
    /// </summary>
    public class Desk
    {
        public string Root { get; set; }

        public IReadOnlyCollection<DeskTile> Items;
    }
}