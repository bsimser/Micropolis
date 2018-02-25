namespace MicropolisCore
{
    /// <summary>
    /// Main definition for the Micropolis class that everything drives off of.
    /// This holds all the variables for the class and the main entry points.
    /// </summary>
    public partial class Micropolis
    {
        /// <summary>
        /// Size of the world in horizontal direction.
        /// </summary>
        public const int WORLD_W = 120;

        /// <summary>
        /// Size of the world in vertical direction.
        /// </summary>
        public const int WORLD_H = 100;

        /// <summary>
        /// The number of bits per tile.
        /// </summary>
        private const int BITS_PER_TILE = 16;

        /// <summary>
        /// The number of bytes per tile.
        /// </summary>
        private const int BYTES_PER_TILE = 2;

        /// <summary>
        /// Horizontal size of the world for a map that stores a value for every 2x2
        /// square.
        /// </summary>
        private const int WORLD_W_2 = WORLD_W / 2;

        /// <summary>
        /// Vertical size of the world for a map that stores a value for every 2x2
        /// square.
        /// </summary>
        private const int WORLD_H_2 = WORLD_H / 2;

        /// <summary>
        /// Horizontal size of the world for a map that stores a value for every 4x4
        /// square.
        /// </summary>
        private const int WORLD_W_4 = WORLD_W / 4;

        /// <summary>
        /// Vertical size of the world for a map that stores a value for every 4x4
        /// square.
        /// </summary>
        private const int WORLD_H_4 = WORLD_H / 4;

        /// <summary>
        /// Horizontal size of the world for a map that stores a value for every 8x8
        /// square.
        /// </summary>
        private const int WORLD_W_8 = WORLD_W / 8;

        /// <summary>
        /// Vertical size of the world for a map that stores a value for every 8x8
        /// square.
        /// </summary>
        private const int WORLD_H_8 = (WORLD_H + 7) / 8;

        /// <summary>
        /// The size of the editor view tiles, in pixels.
        /// </summary>
        private const int EDITOR_TILE_SIZE = 16;

        /// <summary>
        /// The number of simulator passes per #cityTime unit.
        /// </summary>
        public const int PASSES_PER_CITYTIME = 16;

        /// <summary>
        /// The number of #cityTime units per month.
        /// </summary>
        private const int CITYTIMES_PER_MONTH = 4;

        /// <summary>
        /// The number of #cityTime units per year.
        /// </summary>
        private const int CITYTIMES_PER_YEAR = CITYTIMES_PER_MONTH * 12;

        /// <summary>
        /// The number of history entries.
        /// </summary>
        private const int HISTORY_LENGTH = 480;

        /// <summary>
        /// The number of miscellaneous history entries.
        /// </summary>
        private const int MISC_HISTORY_LENGTH = 240;

        /// <summary>
        /// Length of the history tables.
        /// </summary>
        private const int HISTORY_COUNT = 120;

        /// <summary>
        /// The size of the power stack.
        /// </summary>
        private const int POWER_STACK_SIZE = (WORLD_W * WORLD_H) / 4;

        /// <summary>
        /// A constant used in place of an x or y position to indicate
        /// "nowhere".
        /// </summary>
        private const int NOWHERE = -1;

        /// <summary>
        /// Maximal number of map tiles to drive, looking for a destination
        /// </summary>
        private const int MAX_TRAFFIC_DISTANCE = 30;

        /// <summary>
        /// Maximal value of roadEffect
        /// </summary>
        private const int MAX_ROAD_EFFECT = 32;

        /// <summary>
        /// Maximal value of policeEffect
        /// </summary>
        private const int MAX_POLICE_STATION_EFFECT = 1000;

        /// <summary>
        /// Maximal value of fireEffect
        /// </summary>
        private const int MAX_FIRE_STATION_EFFECT = 1000;

        private const int RES_VALVE_RANGE = 2000;
        private const int COM_VALVE_RANGE = 1500;
        private const int IND_VALVE_RANGE = 1500;

        /// <summary>
        /// The default radius of an island, used by the terrain generator.
        /// </summary>
        private const int ISLAND_RADIUS = 18;

        /// <summary>
        /// Number of road tiles in the game.
        /// 
        /// Bridges count as 4 tiles, and high density traffic counts as
        /// 2 tiles.
        /// </summary>
        private short roadTotal;

        /// <summary>
        /// Total number of rails.
        /// 
        /// No penalty for bridges or high traffic density.
        /// </summary>
        private short railTotal;

        /// <summary>
        /// Number of fires.
        /// </summary>
        private short firePop;

        /// <summary>
        /// Number of people in the residential zones.
        /// 
        /// Depends on level of zone development.
        /// </summary>
        private short resPop;

        /// <summary>
        /// Commercial zone population.
        /// 
        /// Depends on level of zone development.
        /// </summary>
        private short comPop;

        /// <summary>
        /// Industrial zone population.
        /// 
        /// Depends on level of zone development.
        /// </summary>
        private short indPop;

        /// <summary>
        /// Total population.
        /// 
        /// Includes residential pop / 8 plus industrial pop plus commerical
        /// pop.
        /// </summary>
        private short totalPop;

        /// <summary>
        /// Last total population.
        /// </summary>
        private short totalPopLast;

        /// <summary>
        /// Number of residential zones.
        /// </summary>
        private short resZonePop;

        /// <summary>
        /// Number of commercial zones.
        /// </summary>
        private short comZonePop;

        /// <summary>
        /// Number of industrial zones.
        /// </summary>
        private short indZonePop;

        /// <summary>
        /// Total zone population.
        /// </summary>
        private short totalZonePop;

        /// <summary>
        /// Number of hospitals.
        /// </summary>
        private short hospitalPop;

        /// <summary>
        /// Number of churches.
        /// </summary>
        private short churchPop;

        /// <summary>
        /// Faith bias.
        /// </summary>
        private short faith;

        /// <summary>
        /// Number of stadiums.
        /// </summary>
        private short stadiumPop;

        /// <summary>
        /// Police station population.
        /// </summary>
        private short policeStationPop;

        /// <summary>
        /// Fire station population.
        /// </summary>
        private short fireStationPop;

        /// <summary>
        /// Coal power plant population.
        /// </summary>
        private short coalPowerPop;

        /// <summary>
        /// Nuclear power plant population.
        /// </summary>
        private short nuclearPowerPop;

        /// <summary>
        /// Seaport population.
        /// </summary>
        private short seaportPop;

        /// <summary>
        /// Airport population.
        /// </summary>
        private short airportPop;

        /// <summary>
        /// Average crime.
        /// 
        /// Affected by land value, population density, police station
        /// distance.
        /// </summary>
        private short crimeAverage;

        /// <summary>
        /// Average pollution.
        /// 
        /// Affected by PollutionMem, which is effected by traffic, fire,
        /// radioactivity, industrial zones, seaports, airports, power
        /// plants.
        /// </summary>
        private short pollutionAverage;

        /// <summary>
        /// Land value average.
        /// 
        /// Affected by distance from city center, development density
        /// (terrainMem), pollution, and crime.
        /// </summary>
        private short landValueAverage;

        /// <summary>
        /// City time unit counter, increnented once every 16 runs through
        /// the simulator (at fast speed). A time unit is 7.6 days. 4 units
        /// per month, 48 units per year, relative to startingYear
        /// 
        /// Four units per month, so one unit is about a week(7.6 days).
        /// </summary>
        public long cityTime;

        /// <summary>
        /// City month, 4 time units per month.
        /// </summary>
        public long cityMonth;

        /// <summary>
        /// City year, (cityTime / 48) + startingYear.
        /// </summary>
        public long cityYear;

        /// <summary>
        /// City starting year.
        /// </summary>
        public short startingYear;

        /// <summary>
        /// Two-dimensional array of map tiles.
        /// 
        /// Map[120][100]
        /// </summary>
        public ushort[,] map;

        /// <summary>
        /// 10 year residential history maximum value.
        /// </summary>
        private short[] resHist10Max;

        /// <summary>
        /// 120 year residential history maximum value.
        /// </summary>
        private short[] resHist120Max;

        /// <summary>
        /// 10 year commercial history maximum value.
        /// </summary>
        private short[] comHist10Max;

        /// <summary>
        /// 120 year commercial history maximum value.
        /// </summary>
        private short[] comHist120Max;

        /// <summary>
        /// 10 year industrial history maximum value.
        /// </summary>
        private short[] indHist10Max;

        /// <summary>
        /// 120 year industrial history maximum value.
        /// </summary>
        private short[] indHist120Max;

        /// <summary>
        /// Census changed flag.
        /// 
        /// Need to redraw census dependent stuff.
        /// </summary>
        private bool censusChanged;

        /// <summary>
        /// Spending on roads.
        /// </summary>
        private long roadSpend;

        /// <summary>
        /// Spending on police stations.
        /// </summary>
        private long policeSpend;

        /// <summary>
        /// Spending on fire stations.
        /// </summary>
        private long fireSpend;

        /// <summary>
        /// Requested funds for roads.
        /// 
        /// Depends on number of roads, rails, and game level.
        /// </summary>
        private long roadFund;

        /// <summary>
        /// Requested funds for police stations.
        /// 
        /// Depends on police station population.
        /// </summary>
        private long policeFund;

        /// <summary>
        /// Requested funds for fire stations.
        /// 
        /// Depends on fire station population.
        /// </summary>
        private long fireFund;

        /// <summary>
        /// Ratio of road spending over road funding, times #MAX_ROAD_EFFECT.
        /// </summary>
        private long roadEffect;

        /// <summary>
        /// Ratio of police spending over police funding, times #MAX_POLICE_EFFECT.
        /// </summary>
        private long policeEffect;

        /// <summary>
        /// Ratio of fire spending over fire funding, times #MAX_FIRE_EFFECT.
        /// </summary>
        private long fireEffect;

        /// <summary>
        /// Funds from taxes.
        /// 
        /// Depends on total population, average land value, city tax, and
        /// game level.
        /// </summary>
        private long taxFund;

        /// <summary>
        /// City tax rate.
        /// </summary>
        public short cityTax;

        /// <summary>
        /// Tax port flag.
        /// </summary>
        private bool taxFlag;

        /// <summary>
        /// Population density map.
        /// </summary>
        private MapByte2 populationDensityMap;

        /// <summary>
        /// Traffic density map.
        /// </summary>
        private MapByte2 trafficDensityMap;

        /// <summary>
        /// Pollution density map.
        /// </summary>
        private MapByte2 pollutionDensityMap;

        /// <summary>
        /// Land value map.
        /// </summary>
        private MapByte2 landValueMap;

        /// <summary>
        /// Crime rate map.
        /// </summary>
        private MapByte2 crimeRateMap;

        /// <summary>
        /// Terrain development density map.
        /// 
        /// Used to calculate land value.
        /// </summary>
        private MapByte4 terrainDensityMap;

        /// <summary>
        /// Temporary map 1.
        /// 
        /// Used to smooth population density, pollution.
        /// </summary>
        private MapByte4 tempMap1;

        /// <summary>
        /// Temporary map 2.
        /// 
        /// Used to smooth population density, pollution.
        /// </summary>
        private MapByte2 tempMap2;

        /// <summary>
        /// Temporary map 3.
        /// 
        /// Used to smooth development density, for terrainDensityMap.
        /// </summary>
        private MapByte4 tempMap3;

        /// <summary>
        /// Power grid map.
        /// </summary>
        private MapByte1 powerGridMap;

        /// <summary>
        /// Rate of growth map.
        /// 
        /// Affected by DecROGMem, incROG called by zones.Decreased by fire
        /// explosions from sprites, fire spreading. Doesn't seem to
        /// actually feed back into the simulation. Output only.
        /// </summary>
        private MapShort8 rateOfGrowthMap;

        /// <summary>
        /// Fire station map.
        /// 
        /// Effectivity of fire control in this area.
        /// 
        /// Affected by fire stations, powered, fire funding ratio, road
        /// access.Affects how long fires burn.
        /// </summary>
        private MapShort8 fireStationMap;

        /// <summary>
        /// Copy of fire station map to display.
        /// </summary>
        private MapShort8 fireStationEffectMap;

        /// <summary>
        /// Police station map.
        /// 
        /// Effectivity of police in fighting crime.
        /// 
        /// Affected by police stations, powered, police funding ratio, road
        /// access. Affects crime rate.
        /// </summary>
        private MapShort8 policeStationMap;

        /// <summary>
        /// Copy of police station map to display.
        /// </summary>
        private MapShort8 policeStationEffectMap;

        /// <summary>
        /// Commercial rate map.
        /// 
        /// Depends on distance to city center.Affects commercial zone
        /// evaluation.
        /// </summary>
        private MapShort8 comRateMap;

        /// <summary>
        /// Residential population history.
        /// </summary>
        private short[] resHist;

        /// <summary>
        /// Commercial population history.
        /// </summary>
        private short[] comHist;

        /// <summary>
        /// Industrial population history.
        /// </summary>
        private short[] indHist;

        /// <summary>
        /// Money history.
        /// </summary>
        private short[] moneyHist;

        /// <summary>
        /// Pollution history.
        /// </summary>
        private short[] pollutionHist;

        /// <summary>
        /// Crime history.
        /// </summary>
        private short[] crimeHist;

        /// <summary>
        /// Memory used to save miscelaneous game values in save file.
        /// </summary>
        private short[] miscHist;

        /// <summary>
        /// Need hospital?
        /// 
        /// 0 if no, 1 if yes, -1 if too many.
        /// </summary>
        private short needHospital;

        /// <summary>
        /// Need church?
        /// 
        /// 0 if no, 1 if yes, -1 if too many.
        /// </summary>
        private short needChurch;

        /// <summary>
        /// Memory for map array.
        /// </summary>
        private short[] mapBase;

        /// <summary>
        /// Percentage of requested road and rail costs to funding level.
        /// 
        /// Value between \c 0 and ::MAX_ROAD_EFFECT.
        /// Affected by road funds slider and budgetary constraints.
        /// </summary>
        private float roadPercent;

        /// <summary>
        /// Percentage of requested police station costs to funding level.
        /// 
        /// Value between \c 0 and ::MAX_POLICESTATION_EFFECT.
        /// Affected by road funds slider and budgetary constraints.
        /// </summary>
        private float policePercent;

        /// <summary>
        /// Percentage of requested fire station costs to funding level.
        /// 
        /// Value between \c 0 and ::MAX_FIRESTATION_EFFECT.
        /// Affected by road funds slider and budgetary constraints.
        /// </summary>
        private float firePercent;

        /// <summary>
        /// Amount of road funding granted.
        /// </summary>
        private long roadValue;

        /// <summary>
        /// Amount of police funding granted.
        /// </summary>
        private long policeValue;

        /// <summary>
        /// Amount of fire station funding granted.
        /// </summary>
        private long fireValue;

        /// <summary>
        /// Flag set when budget window needs to be updated.
        /// </summary>
        private int mustDrawBudget;

        /// <summary>
        /// Size of flooding disaster.
        /// </summary>
        private short floodCount;

        /// <summary>
        /// Yes votes.
        /// 
        /// Percentage of people who think the mayor is doing a good job.
        /// </summary>
        public short cityYes;

        // short problemVotes[PROBNUM];
        // short probelmOrder[CVP_PROBLEM_COMPLAINTS];

        /// <summary>
        /// City population.
        /// 
        /// Depends of ResPop, ComPop and IndPop.
        /// </summary>
        public long cityPop;

        /// <summary>
        /// Change in the city population.
        /// 
        /// Depends on last cityPop.
        /// </summary>
        public long cityPopDelta;

        /// <summary>
        /// City assessed value.
        /// 
        /// Depends on roadTotal, railTotal, policeStationPop,
        /// fireStationPop, hospitalPop, stadiumPop, seaportPop,
        /// airportPop, coalPowerPop, and nuclearPowerPop, and their
        /// respective values.
        /// </summary>
        public long cityAssessedValue;

        private CityClass cityClass;

        /// <summary>
        /// City score.
        /// 
        /// Affected by average of problems, residential cap, commercial cap,
        /// industrial cap, road effect, police effect, fire effect,
        /// residential valve, commercial valve, industrial valve, city
        /// population, delta city population, fires, tax rate, and unpowered
        /// zones.
        /// </summary>
        private short cityScore;

        /// <summary>
        /// Change in the city score.
        /// 
        /// Depends on city score.
        /// </summary>
        private short cityScoreDelta;

        /// <summary>
        /// Average traffic.
        /// 
        /// Depends on average traffic density of tiles with non-zero land value.
        /// </summary>
        private short trafficAverage;

        /// <summary>
        /// Controls the level of tree creation.
        /// -1 => create default number of trees, 0 => never create trees, >0 => create more trees
        /// </summary>
        private int terrainTreeLevel;

        /// <summary>
        /// Controls the level of lake creation.
        /// -1 => create default number of lakes, 0 => never create lakes, >0 => create more lakes
        /// </summary>
        private int terrainLakeLevel;

        /// <summary>
        /// Controls the level of river curviness.
        /// -1 => default curve level, 0 => never create rivers, >0 => create curvier rivers
        /// </summary>
        private int terrainCurveLevel;

        /// <summary>
        /// Controls how often to create an island.
        /// -1 => 10% chance of island, 0 => never create island, 1 => always create island
        /// </summary>
        private int terrainCreateIsland;

        /// <summary>
        /// The seed of the most recently generated city.
        /// </summary>
        private int generatedCitySeed;

        private bool historyInitialized;
        private short graph10Max;
        private short graph120Max;

        private int simLoops;

        /// <summary>
        /// The number of passes through the simulator loop to take each tick.
        /// </summary>
        public int simPasses;

        /// <summary>
        /// The count of the current pass through the simulator loop.
        /// </summary>
        private int simPass;

        /// <summary>
        /// Simulation is paused
        /// </summary>
        private bool simPaused;

        private int simPausedSpeed;
        protected bool tilesAnimated;
        protected bool doAnimation;
        private bool doMessages;
        private bool doNotices;

        /// <summary>
        /// Filename of the last loaded city
        /// </summary>
        private string cityFileName;

        /// <summary>
        /// Name of the city.
        /// </summary>
        public string cityName;

        private int heatSteps;
        private int heatFlow;
        private int heatRule;
        private int heatWrap;

        private short[] cellSrc;
        private short[] cellDst;

        /// <summary>
        /// Population of last city class check.
        /// </summary>
        private long cityPopLast;

        /// <summary>
        /// City class of last city class check.
        /// </summary>
        private short categoryLast;

        /// <summary>
        /// Enable auto goto
        /// 
        /// When enabled and an important event happens, the map display will jump to
        /// the location of the event
        /// </summary>
        private bool autoGoto;

        /// <summary>
        /// Power stack
        /// Stack used to find powered tiles by tracking conductive tiles.
        /// Stack counter, points to the top-most item.
        /// </summary>
        private int powerStackPointer;

        //private Position powerStackXY[POWER_STACK_SIZE];

        private long nextRandom;

        /// <summary>
        /// Name of the Micropolis top level home directory.
        /// </summary>
        private string homeDir;

        /// <summary>
        /// Name of the sub-directory where the resources are located.
        /// </summary>
        private string resourceDir;
        
        //Resource *resources
        //StringTable *stringTables

        private short newMap;

        //private short newMapFlags[MAP_TYPE_COUNT];

        /// <summary>
        /// X coordinate of city center
        /// </summary>
        private short cityCenterX;

        /// <summary>
        /// Y coordinate of city center
        /// </summary>
        private short cityCenterY;

        /// <summary>
        /// X coordinate of most polluted area
        /// </summary>
        private short pollutionMaxX;

        /// <summary>
        /// Y coordinate of most polluted area
        /// </summary>
        private short pollutionMaxY;

        /// <summary>
        /// X coordinate of most criminal area.
        /// </summary>
        private short crimeMaxX;

        /// <summary>
        /// Y coordinate of most criminal area.
        /// </summary>
        private short crimeMaxY;

        /// <summary>
        /// Integer with bits 0..2 that control smoothing.
        /// </summary>
        private long donDither;

        private bool valveFlag;
        private short crimeRamp;
        private short pollutionRamp;

        /// <summary>
        /// Block residential growth
        /// </summary>
        private bool resCap;

        /// <summary>
        /// Block commercial growth
        /// </summary>
        private bool comCap;

        /// <summary>
        /// Block industrial growth
        /// </summary>
        private bool indCap;

        private short cashFlow;
        private float externalMarket;

        //private Scenario disasterEvent;

        /// <summary>
        /// Count-down timer for the disaster
        /// </summary>
        private short disasterWait;

        //private Scenario scoreType;

        /// <summary>
        /// Time to wait before computing the score
        /// </summary>
        private short scoreWait;

        /// <summary>
        /// Number of powered tiles in all zone
        /// </summary>
        private short poweredZoneCount;

        /// <summary>
        /// Number of unpowered tiles in all zones
        /// </summary>
        private short unpoweredZoneCount;

        private bool newPower;
        private short cityTaxAverage;
        private short simCycle;
        private short phaseCycle;
        private short speedCycle;

        /// <summary>
        /// Need to perform initial city evaluation.
        /// </summary>
        private bool doInitialEval;

        /// <summary>
        /// The invalidateMaps method increases the map serial number every time the maps changes.
        /// </summary>
        private int mapSerial;

        private short resValve;
        private short comValve;
        private short indValve;

        /// <summary>
        /// List of active sprites.
        /// </summary>
        private SimSprite[] spriteList;

        /// <summary>
        /// Pool of free SimSprite objects.
        /// </summary>
        private SimSprite[] freeSprites;

        // TODO private SimSprite[] globalSprites[SPRITE_COUNT];

        private int absDist;
        private short spriteCycle;

        /// <summary>
        /// Funds of the player
        /// </summary>
        public long totalFunds;

        /// <summary>
        /// Enable auto-bulldoze
        /// 
        /// When enabled, the game will silently clear tiles when the user
        /// builds something on non-clear and bulldozable tiles
        /// </summary>
        private bool autoBulldoze;

        /// <summary>
        /// Enable auto budget
        /// 
        /// When enabled, the program will perform budgetting of the city
        /// </summary>
        private bool autoBudget;
        private long messageTimeLast;
        //private GameLevel gameLevel;
        private short initSimLoad;
        //private Scenario scenario;
        public short simSpeed;
        private short simSpeedMeta;

        /// <summary>
        /// Enable sound
        /// </summary>
        private bool enableSound;

        /// <summary>
        /// Enable disasters
        /// </summary>
        private bool enableDisasters;
        private short messageNumber;

        /// <summary>
        /// The evaluation window should be shown to the user
        /// </summary>
        private bool evalChanged;

        private short blinkFlag;
        // CallbackFunction callbackHook
        // void *callbackData
        // void *userData;

        private bool mustUpdateFunds;

        /// <summary>
        /// Options displayed at user need updating.
        /// </summary>
        private bool mustUpdateOptions;

        private long cityTimeLast;
        public long cityYearLast;
        public long cityMonthLast;
        private long totalFundsLast;
        private long resLast;
        private long comLast;
        private long indLast;

        /// <summary>
        /// Simualtor constructor.
        /// </summary>
        public Micropolis()
        {
            // TODO initialize all objects like pouplationDensityMap, crimeRateMap, powerGridMap, etc.
            init();
        }

        /// <summary>
        /// Initialize simulator variables to a sane default.
        /// </summary>
        private void init()
        {
            roadTotal = 0;
            railTotal = 0;
            firePop = 0;
            resPop = 0;
            comPop = 0;
            indPop = 0;
            totalPop = 0;
            totalPopLast = 0;
            resZonePop = 0;
            comZonePop = 0;
            indZonePop = 0;
            totalZonePop = 0;
            hospitalPop = 0;
            churchPop = 0;
            faith = 0;
            stadiumPop = 0;
            policeStationPop = 0;
            fireStationPop = 0;
            coalPowerPop = 0;
            nuclearPowerPop = 0;
            seaportPop = 0;
            airportPop = 0;
            needHospital = 0;
            needChurch = 0;
            crimeAverage = 0;
            pollutionAverage = 0;
            landValueAverage = 0;
            cityTime = 0;
            cityMonth = 0;
            cityYear = 0;
            startingYear = 0;
            //resHist10Max = 0;
            //resHist120Max = 0;
            //comHist10Max = 0;
            //comHist120Max = 0;
            //indHist10Max = 0;
            //indHist120Max = 0;
            censusChanged = false;
            roadSpend = 0;
            policeSpend = 0;
            fireSpend = 0;
            roadFund = 0;
            policeFund = 0;
            fireFund = 0;
            roadEffect = 0;
            policeEffect = 0;
            fireEffect = 0;
            taxFund = 0;
            cityTax = 0;
            taxFlag = false;
            // populationDensityMap.clear();
            // etc
            // mapBase
            // resHist
            // comHist
            // indHist
            // moneyHist
            // pollutionHist
            // crimeHist
            // miscHist
            roadPercent = 0.0f;
            policePercent = 0.0f;
            firePercent = 0.0f;
            roadValue = 0;
            policeValue = 0;
            fireValue = 0;
            mustDrawBudget = 0;
            floodCount = 0;
            cityYes = 0;
            cityPop = 0;
            cityPopDelta = 0;
            cityAssessedValue = 0;
            // cityClass = CC_VILLAGE;
            cityScore = 0;
            cityScoreDelta = 0;
            trafficAverage = 0;
            terrainTreeLevel = -1;
            terrainLakeLevel = -1;
            terrainCurveLevel = -1;
            terrainCreateIsland = -1;
            graph10Max = 0;
            graph120Max = 0;
            simLoops = 0;
            simPasses = 0;
            simPass = 0;
            simPaused = false;
            simPausedSpeed = 3;
            heatSteps = 0;
            heatFlow = -7;
            heatRule = 0;
            heatWrap = 3;
            cityFileName = "";
            tilesAnimated = false;
            doAnimation = true;
            doMessages = true;
            doNotices = true;
            // cellSrc
            // cellDst
            cityPopLast = 0;
            categoryLast = 0;
            autoGoto = false;
            powerStackPointer = 0;

            nextRandom = 1;
            homeDir = "";
            resourceDir = "";

            newMap = 0;

            cityCenterX = 0;
            cityCenterY = 0;
            pollutionMaxX = 0;
            pollutionMaxY = 0;
            crimeMaxX = 0;
            crimeMaxY = 0;
            donDither = 0;

            valveFlag = false;
            crimeRamp = 0;
            pollutionRamp = 0;

            resCap = false; // Do not block residential growth
            comCap = false; // Do not block commercial growth
            indCap = false; // Do not block industrial growth

            cashFlow = 0;
            disasterWait = 0;
            scoreWait = 0;
            poweredZoneCount = 0;
            unpoweredZoneCount = 0;
            newPower = false;
            cityTaxAverage = 0;
            simCycle = 0;
            phaseCycle = 0;
            speedCycle = 0;
            doInitialEval = false;
            mapSerial = 1;
            resValve = 0;
            comValve = 0;
            indValve = 0;

            absDist = 0;
            spriteCycle = 0;
            totalFunds = 0;
            autoBulldoze = true;
            autoBudget = true;

            initSimLoad = 0;
            simSpeed = 0;
            simSpeedMeta = 0;
            enableSound = false;
            enableDisasters = true;
            evalChanged = false;
            blinkFlag = 0;

            mustUpdateFunds = false;
            mustUpdateOptions = false;
            cityTimeLast = 0;
            cityYearLast = 0;
            cityMonthLast = 0;
            totalFundsLast = 0;
            resLast = 0;
            comLast = 0;
            indLast = 0;

            simInit();
        }

        public void destroy()
        {
            destroyMapArrays();
        }

        /// <summary>
        /// Generate a random MapTileCharacters::RUBBLE tile
        /// </summary>
        /// <returns></returns>
        private ushort randomRubble()
        {
            return(ushort)((ushort)((ushort)MapTileCharacters.RUBBLE + (getRandom16() & 3)) | (ushort) MapTileBits.BULLBIT);
        }
    }
}
