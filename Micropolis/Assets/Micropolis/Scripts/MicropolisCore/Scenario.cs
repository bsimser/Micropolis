namespace Micropolis.MicropolisCore
{
    /// <summary>
    /// Available scenarios
    /// </summary>
    public enum Scenario
    {
        SC_NONE,           // No scenario (free playing)

        SC_DULLSVILLE,     // Dullsville (boredom)
        SC_SAN_FRANCISCO,  // San francisco (earthquake)
        SC_HAMBURG,        // Hamburg (fire bombs)
        SC_BERN,           // Bern (traffic)
        SC_TOKYO,          // Tokyo (scary monster)
        SC_DETROIT,        // Detroit (crime)
        SC_BOSTON,         // Boston (nuclear meltdown)
        SC_RIO,            // Rio (flooding)

        SC_COUNT,          // Number of scenarios  
    }
}