namespace Micropolis.MicropolisCore
{
    /// <summary>
    /// Available map types.
    /// </summary>
    public enum MapType
    {
        MAP_TYPE_ALL,                   // All zones
        MAP_TYPE_RES,                   // Residential zones
        MAP_TYPE_COM,                   // Commercial zones
        MAP_TYPE_IND,                   // Industrial zones
        MAP_TYPE_POWER,                 // Power connectivity
        MAP_TYPE_ROAD,                  // Roads
        MAP_TYPE_POPULATION_DENSITY,    // Population density
        MAP_TYPE_RATE_OF_GROWTH,        // Rate of growth
        MAP_TYPE_TRAFFIC_DENSITY,       // Traffic
        MAP_TYPE_POLLUTION,             // Pollution
        MAP_TYPE_CRIME,                 // Crime rate
        MAP_TYPE_LAND_VALUE,            // Land value
        MAP_TYPE_FIRE_RADIUS,           // Fire station coverage radius
        MAP_TYPE_POLICE_RADIUS,         // Police station coverage radius
        MAP_TYPE_DYNAMIC,               // Dynamic filter

        MAP_TYPE_COUNT,                 // Number of map types
    }
}