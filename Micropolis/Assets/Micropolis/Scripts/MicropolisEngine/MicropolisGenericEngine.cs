using UnityEngine;

namespace MicropolisEngine
{
    public class MicropolisGenericEngine : MicropolisCore.Micropolis
    {
        public MicropolisGenericEngine()
        {
            // resourceDir
            // running
            // timeDelay
            // timerActive = false;
            // timerId = None
            // views = []
            // interests = {}
            // residentialImage = None
            // commercialImage = None
            // industrialImage = None
            // transportationImage = None
            // populationDensityImage = None
            // rateOfGrowthImage = None
            // landValueImage = None
            // crimeRateImage = None
            // pollutionDensityImage = None
            // trafficDensityImage = None
            // powerGridImage = None
            // fireCoverageImage = None
            // policeCoverageImage = None
            // dateTileEngine = new TileEngine();
            // robots = []
            // robotDict = {}
            // zoneMap = {}
            // saveFileDir = None
            // saveFileName = None
            // metaFileName = None
            // title = "";
            // description = "";
            // readOnly = false;
            // gameMode = 'stopped';

            // Hook the engine up so it has a handle on it's C# object side
            // userData = getPythonCallbackData(this)

            // Hook up the callback mechanism to our callback handler
            // callbackHook = getPythonCallbackHook();

            initGame();

            // if(running)
            //   startTimer();

            Debug.Log("MicropolisEngine.__init__ done");
        }

        public void initGameUnity()
        {
            Debug.Log("MicropoligGenericEngine initGameUnity: This should be called at the end of the concrete subclass's __init__ method.");
        }
    }
}