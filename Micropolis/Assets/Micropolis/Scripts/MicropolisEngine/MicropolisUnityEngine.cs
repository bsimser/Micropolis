using System.Collections.Generic;
using MicropolisCore;

namespace MicropolisEngine
{
    /// <summary>
    /// Platform specific version of the engine that handles anything
    /// Unity specific like panels and UI elements. Also holds the 
    /// UI text for the game messages (could be loaded from resources).
    /// This is aligned to what the original MicropolisGTKEngine did.
    /// </summary>
    public class MicropolisUnityEngine : MicropolisGenericEngine
    {
        public float zoom;

        public const float MAX_ZOOM = 10f;

        public const float MIN_ZOOM = 20f;

        /// <summary>
        /// String representation of <see cref="MessageNumber"/>
        /// </summary>
        public string[] messages =
        {
            "",
            "More residential zones needed.",
            "More commercial zones needed.",
            "More industrial zones needed.",
            "More roads required.",
            "Inadequate rail system.",
            "Build a power plant.",
            "Residents demand a stadium.",
            "Industry requires a sea port.",
            "Commerce requires an airport.",
            "Pollution very high.",
            "Crime very high.",
            "Frequent traffic jams reported.",
            "Citizens demand a fire department.",
            "Citizens demand a police department.",
            "Blackouts reported. Check power map.",
            "Citizens upset. The tax rate is too high.",
            "Roads deteriorating, due to lack of funds.",
            "Fire departments need funding.",
            "Police departments need funding.",
            "Fire reported !",
            "A monster has been sighted !!",
            "Tornado reported !!",
            "Major earthquake reported !!!",
            "A plane has crashed !",
            "Shipwreck reported !",
            "A train crashed !",
            "A helicopter crashed !",
            "Unemployment rate is high.",
            "YOUR CITY HAS GONE BROKE!",
            "Firebombing reported !", // TODO original message "Bulldozing too many trees."
            "Need more parks.",
            "Explosion detected !",
            "Insufficient funds to build that.",
            "Area must be bulldozed first.",
            "Population has reached 2,000.",
            "Population has reached 10,000.",
            "Population has reached 50,000.",
            "Population has reached 100,000.",
            "Population has reached 500,000.",
            "Brownouts, build another power plant.",
            "Heavy traffic reported.",
            "Flooding reported !!",
            "A nuclear meltdown has occurred !!!",
            "They're rioting in the streets !!", // TODO original message "Cannot build on water."
            "Started a new city.", // TODO original message "Cannot build that here."
            "Restored a saved city.", // TODO original message "Cannot bulldoze here."
            "You won the scenario!",
            "You lost the scenario.",
            "About Micropolis.",
            "Scenario: DULLSVILLE, USA  1900.",
            "Scenario: SAN FRANCISCO, CA.  1906.",
            "Scenario: HAMBURG, GERMANY  1944.",
            "Scenario: BERN, SWITZERLAND  1965.",
            "Scenario: TOKYO, JAPAN  1957.",
            "Scenario: DETROIT, MI.  1972.",
            "Scenario: BOSTON, MA.  2010.",
            "Scenario: RIO DE JANEIRO, BRAZIL  2047.",
        };

        public string[] editorIconMessages =
        {
            "Bulldoze: $1",
            "Road: $10",
            "Power lines: $5",
            "Railroad: $20",
            "Park: $10",
            "Residential: $100",
            "Commercial: $100",
            "Industrial: $100",
            "Police station: $500",
            "Fire station: $500",
            "Stadium: $3000",
            "Nuclear reactor: $5000",
            "Seaport: $5000",
            "Airport: $10000",
            "Coal power plant: $3000"
        };

        public List<GraphicTileset> tilesets = new List<GraphicTileset>
        {
            new GraphicTileset
            {
                FolderName = "classic",
                Title = "Classic Graphics (DOS)"
            },
            new GraphicTileset
            {
                FolderName = "classic95",
                Title = "Classic Graphics (Windows)"
            },
            new GraphicTileset
            {
                FolderName = "ancientasia",
                Title = "Ancient Asia"
            },
            new GraphicTileset
            {
                FolderName = "futureeurope",
                Title = "Future Europe"
            },
            new GraphicTileset
            {
                FolderName = "futureusa",
                Title = "Future USA"
            },
            new GraphicTileset
            {
                FolderName = "medievaltimes",
                Title = "Medieval Times"
            },
            new GraphicTileset
            {
                FolderName = "mooncolony",
                Title = "Moon Colony"
            },
            new GraphicTileset
            {
                FolderName = "wildwest",
                Title = "Wild West"
            },
            new GraphicTileset
            {
                FolderName = "x11",
                Title = "Classic Graphics (X11)"
            }
        };

        // TODO list of notices to show to user

        public List<Scenario> scenarios = new List<Scenario>
        {
            new Scenario
            {
                id = ScenarioType.SC_DULLSVILLE,
                title = "Dullsville, USA  1900",
                description = "Things haven't changed much around here in the last hundred years or so and the residents are beginning to get bored. They think Dullsville could be the next great city with the right leader.\r\n" +
                              "It is your job to attract new growth and development, turning Dullsville into a Metropolis within 30 years."
            },
            new Scenario
            {
                id = ScenarioType.SC_SAN_FRANCISCO,
                title = "San Francisco, CA.  1906",
                description = "Damage from the earthquake was minor compared to that of the ensuing fires, which took days to control. 1500 people died.\r\n" +
                              "Controlling the fires should be your initial concern. Then clear the rubble and start rebuilding. You have 5 years."
            },
            new Scenario
            {
                id = ScenarioType.SC_HAMBURG,
                title = "Hamburg, Germany  1944",
                description = "Allied fire-bombing of German cities in WWII caused tremendous damage and loss of life. People living in the inner cities were at greatest risk.\r\n" +
                              "You must control the firestorms during the bombing and then rebuild the city after the war. You have 5 years."
            },
            new Scenario
            {
                id = ScenarioType.SC_BERN,
                title = "Bern, Switzerland  1965",
                description = "The roads here are becoming more congested every day, and the residents are upset. They demand that you do something about it.\r\n" +
                              "Some have suggested a mass transit system as the answer, but this would require major rezoning in the downtown area. You have 10 years."
            },
            new Scenario
            {
                id = ScenarioType.SC_TOKYO,
                title = "Tokyo, Japan  1957",
                description = "A large reptilian creature has been spotted heading for Tokyo bay. It seems to be attracted to the heavy levels of industrial pollution there.\r\n" +
                              "Try to control the fires, then rebuild the industrial center. You have 5 years."
            },
            new Scenario
            {
                id = ScenarioType.SC_DETROIT,
                title = "Detroit, MI.  1972",
                description = "By 1970, competition from overseas and other economic factors pushed the once \"automobile capital of the world\" into recession. Plummeting land values and unemployment then increased crime in the inner-city to chronic levels.\r\n" +
                              "You have 10 years to reduce crime and rebuild the industrial base of the city."
            },
            new Scenario
            {
                id = ScenarioType.SC_BOSTON,
                title = "Boston, MA.  2010",
                description = "A major meltdown is about to occur at one of the new downtown nuclear reactors. The area in the vicinity of the reactor will be severly contaminated by radiation, forcing you to restructure the city around it.\r\n" +
                              "You have 5 years to get the situation under control."
            },
            new Scenario
            {
                id = ScenarioType.SC_RIO,
                title = "Rio de Janeiro, Brazil  2047",
                description = "In the mid-21st century, the greenhouse effect raised global temperatures 6 degrees F. Polar icecaps melted and raised sea levels worldwide. Coastal areas were devastated by flood and erosion.\r\n" +
                              "You have 10 years to turn this swamp back into a city again."
            },
        };

        public MicropolisUnityEngine() : base()
        {
            // This must be called at the end of the concrete subclass's
            // init, so it happens last.
            initGameUnity();
        }

        // TODO startTimer

        // TODO stopTimer

        // TODO tickTimer

        public void tickEngine()
        {
            simTick();
            if (doAnimation && !tilesAnimated)
            {
                animateTiles();
            }
            simUpdate();

            // sendUpdate('tick')
            // sendUpdate('editor')
            // sendUpdate('map')
        }


        // TODO handle_didGenerateMap

        // TODO handle_didLoadCity

        // TODO handle_didLoadScenario

        // TODO handle_simulateChurch

        public static MicropolisUnityEngine CreateUnityEngine()
        {
            // Get our nice scriptable subclass of the SWIG Micropolis wrapper object.
            var engine = new MicropolisUnityEngine();

            return engine;
        }
    }
}