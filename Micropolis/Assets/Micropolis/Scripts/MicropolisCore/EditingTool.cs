namespace MicropolisCore
{
    /// <summary>
    /// Available tools.
    /// 
    /// These describe the wand values, the object dragged around on the screen.
    /// </summary>
    public enum EditingTool
    {
        TOOL_RESIDENTIAL,
        TOOL_COMMERCIAL,
        TOOL_INDUSTRIAL,
        TOOL_FIRESTATION,
        TOOL_POLICESTATION,
        TOOL_QUERY,
        TOOL_WIRE,
        TOOL_BULLDOZER,
        TOOL_RAILROAD,
        TOOL_ROAD,
        TOOL_STADIUM,
        TOOL_PARK,
        TOOL_SEAPORT,
        TOOL_COALPOWER,
        TOOL_NUCLEARPOWER,
        TOOL_AIRPORT,
        TOOL_NETWORK,
        TOOL_WATER,
        TOOL_LAND,
        TOOL_FOREST,

        TOOL_COUNT,
        TOOL_FIRST = TOOL_RESIDENTIAL,
        TOOL_LAST = TOOL_FOREST,
    }
}