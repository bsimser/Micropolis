using MicropolisCore;
using UnityEngine;

namespace MicropolisEngine
{
    public class MicropolisGenericEngine : Micropolis
    {
        public MicropolisGenericEngine()
        {
            resourceDir = "res";
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

            // self.expressInterest(self, ('paused'))

            initGame();

            // if(running)
            //   startTimer();

            //Debug.Log("MicropolisEngine.__init__ done");
        }

        public void initGameUnity()
        {
            Debug.Log("MicropoligGenericEngine initGameUnity: This should be called at the end of the concrete subclass's __init__ method.");
        }

        // formatMoney

        // formatPercent

        // formatDelta

        // formatNumber

        // addView

        // removeView

        // getMapImage

        // getDataImageAlphaSize

        // getAllImageAlphaSize

        // getResidentialImageAlphaSize

        // getCommercialImageAlphaSize

        // getIndustrialImageAlphaSize

        // getTransportationImageAlphaSize

        // getPopulationDensityImageAlphaSize

        // getRateOfGrowthImageAlphaSize

        // getLandValueImageAlphaSize

        // getCrimeRateImageAlphaSize

        // getPollutionDensityImageAlphaSize

        // getTrafficDensityImageAlphaSize

        // getPowerGridImageAlphaSize

        // getFireCoverageImageAlphaSize

        // getPoliceCoverageImageAlphaSize

        // makeImage

        // startTimer

        // stopTimer

        // update

        // sendUpdate

        // expressInterest

        // revokeInterest

        // loadMetaCity

        // saveMetaCity

        // getMetaData

        // loadMetaScenario

        // generateNewMetaCity

        // setGameMode

        // dumpRobots

        // addRobot

        // removeRobot

        // getRobot

        // clearRobots

        // simRobots

        // simZones

        // isRoad

        // resetRealTime

        // resetCity

        // clearZones

        // addZone

        // removeZone

        // invokeCallback

        // handle_autoGoto

        // handle_didGenerateMap

        // handle_didLoadCity

        // handle_didLoadScenario

        // handle_didSaveCity

        // handle_didTool

        // handle_didntLoadCity

        // handle_didntSaveCity

        // handle_playNewCity

        // handle_makeSound

        // handle_newGame

        // handle_loseGame

        // handle_reallyStartGame

        // handle_saveCityAs

        // handle_showBudgetAndWait

        // handle_showPicture

        // handle_showZoneStatus

        // handle_startEarthquake

        // handle_startScenario

        // handle_startLoad

        // handle_winGame

        // handle_update

        // handle_simRobots

        // handle_invalidateEditors

        // handle_invalidateMaps

        // handle_simulateChurch
    }
}