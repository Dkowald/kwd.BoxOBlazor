using Microsoft.AspNetCore.Components;

namespace ForServer.Tiles
{
    /// <summary>
    /// Item used within a <see cref="TilePanel"/>.
    /// </summary>
    /// <remarks>
    /// Implementations are expected to register with their owning layout.
    /// </remarks>
    public interface ITileItem
    {
        /// <summary>
        /// Provided to child tile when created.
        /// </summary>
        /// <remarks>
        /// Implementations need to re-apply the <see cref="CascadingParameterAttribute"/>
        /// </remarks>
        [CascadingParameter]
        TilePanel Owner { get; set; }

        /// <summary>
        /// Tile unique id.
        /// </summary>
        string Title { get;set; }
    }
}