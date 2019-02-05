namespace MicropolisCore
{
    /// <summary>
    /// String numbers of messages.
    /// </summary>
    public enum MessageNumber
    {
        MESSAGE_NEED_MORE_RESIDENTIAL = 1, // More residential zones needed.
        MESSAGE_NEED_MORE_COMMERCIAL, // More commercial zones needed.
        MESSAGE_NEED_MORE_INDUSTRIAL, // More industrial zones needed.
        MESSAGE_NEED_MORE_ROADS, // More roads required.
        MESSAGE_NEED_MORE_RAILS, // 5: Inadequate rail system.
        MESSAGE_NEED_ELECTRICITY, // Build a Power Plant.
        MESSAGE_NEED_STADIUM, // Residents demand a Stadium.
        MESSAGE_NEED_SEAPORT, // Industry requires a Sea Port.
        MESSAGE_NEED_AIRPORT, // Commerce requires an Airport.
        MESSAGE_HIGH_POLLUTION, // 10: Pollution very high.
        MESSAGE_HIGH_CRIME, // Crime very high.
        MESSAGE_TRAFFIC_JAMS, // Frequent traffic jams reported.
        MESSAGE_NEED_FIRE_STATION, // Citizens demand a Fire Department.
        MESSAGE_NEED_POLICE_STATION, // Citizens demand a Police Department.
        MESSAGE_BLACKOUTS_REPORTED, // 15: Blackouts reported. Check power map.
        MESSAGE_TAX_TOO_HIGH, // Citizens upset. The tax rate is too high.
        MESSAGE_ROAD_NEEDS_FUNDING, // Roads deteriorating, due to lack of funds.
        MESSAGE_FIRE_STATION_NEEDS_FUNDING, // Fire departments need funding.
        MESSAGE_POLICE_NEEDS_FUNDING, // Police departments need funding.
        MESSAGE_FIRE_REPORTED, // 20: Fire reported !
        MESSAGE_MONSTER_SIGHTED, // A Monster has been sighted !!
        MESSAGE_TORNADO_SIGHTED, // Tornado reported !!
        MESSAGE_EARTHQUAKE, // Major earthquake reported !!!
        MESSAGE_PLANE_CRASHED, // A plane has crashed !
        MESSAGE_SHIP_CRASHED, // 25: Shipwreck reported !
        MESSAGE_TRAIN_CRASHED, // A train crashed !
        MESSAGE_HELICOPTER_CRASHED, // A helicopter crashed !
        MESSAGE_HIGH_UNEMPLOYMENT, // Unemployment rate is high.
        MESSAGE_NO_MONEY, // YOUR CITY HAS GONE BROKE!
        MESSAGE_FIREBOMBING, // 30: Firebombing reported !
        MESSAGE_NEED_MORE_PARKS, // Need more parks.
        MESSAGE_EXPLOSION_REPORTED, // Explosion detected !
        MESSAGE_NOT_ENOUGH_FUNDS, // Insufficient funds to build that.
        MESSAGE_BULLDOZE_AREA_FIRST, // Area must be bulldozed first.
        MESSAGE_REACHED_TOWN, // 35: Population has reached 2,000.
        MESSAGE_REACHED_CITY, // Population has reached 10,000.
        MESSAGE_REACHED_CAPITAL, // Population has reached 50,000.
        MESSAGE_REACHED_METROPOLIS, // Population has reached 100,000.
        MESSAGE_REACHED_MEGALOPOLIS, // Population has reached 500,000.
        MESSAGE_NOT_ENOUGH_POWER, // 40: Brownouts, build another Power Plant.
        MESSAGE_HEAVY_TRAFFIC, // Heavy Traffic reported.
        MESSAGE_FLOODING_REPORTED, // Flooding reported !!
        MESSAGE_NUCLEAR_MELTDOWN, // A Nuclear Meltdown has occurred !!!
        MESSAGE_RIOTS_REPORTED, // They're rioting in the streets !!
        MESSAGE_STARTED_NEW_CITY, // 45: Started a New City.
        MESSAGE_LOADED_SAVED_CITY, // Restored a Saved City.
        MESSAGE_SCENARIO_WON,  // You won the scenario
        MESSAGE_SCENARIO_LOST, // You lose the scenario
        MESSAGE_ABOUT_MICROPOLIS, // About micropolis.
        MESSAGE_SCENARIO_DULLSVILLE, // 50: Dullsville scenario.
        MESSAGE_SCENARIO_SAN_FRANCISCO, // San Francisco scenario.
        MESSAGE_SCENARIO_HAMBURG, // Hamburg scenario.
        MESSAGE_SCENARIO_BERN, // Bern scenario.
        MESSAGE_SCENARIO_TOKYO, // Tokyo scenario.
        MESSAGE_SCENARIO_DETROIT, // 55: Detroit scenario.
        MESSAGE_SCENARIO_BOSTON, // Boston scenario.
        MESSAGE_SCENARIO_RIO_DE_JANEIRO, // 57: Rio de Janeiro scenario.

        MESSAGE_LAST = 57, // Last valid message
    }
}