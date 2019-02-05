namespace MicropolisCore
{
    /// <summary>
    /// Connect tile commands.
    /// </summary>
    public enum ConnectTileCommand
    {
        CONNECT_TILE_FIX, // Fix zone (connect wire, road, and rail).
        CONNECT_TILE_BULLDOZE, // Bulldoze and fix zone.
        CONNECT_TILE_ROAD, // Lay road and fix zone.
        CONNECT_TILE_RAILROAD, // Lay rail and fix zone.
        CONNECT_TILE_WIRE, // Lay wire and fix zone.
    }
}