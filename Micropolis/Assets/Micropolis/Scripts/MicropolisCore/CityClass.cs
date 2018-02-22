namespace Micropolis.MicropolisCore
{
    /// <summary>
    /// Available classes of cities
    /// </summary>
    public enum CityClass
    {
        CC_VILLAGE,     // Village
        CC_TOWN,        // Town, > 2000 citizens
        CC_CITY,        // City, > 10000 citizens
        CC_CAPITAL,     // Capital, > 50000 citizens
        CC_METROPOLIS,  // Metropolis, > 100000 citizens
        CC_MEGALOPOLIS, // Megalopolis, > 500000 citizens

        CC_NUM_CITIES,  // Number of city classes 
    }
}