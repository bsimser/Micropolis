namespace Micropolis.MicropolisCore
{
    public partial class Micropolis
    {
        public Micropolis()
        {
            init();
        }

        /// <summary>
        /// Initialize simulator variables to a sane default.
        /// </summary>
        private void init()
        {
            // roadTotal = 0;
            // railTotal = 0;
            // firePop = 0;
            // resPop = 0;
            // compPop = 0;
            // indPop = 0;
            // totalPop = 0;
            // totalPopLast = 0;
            // resZonePop = 0;
            // comZonePop = 0;
            // indZonePop = 0;
            // totalZonePop = 0;
            // hospitalPop = 0;
            // churchPop = 0;
            // faith = 0;
            // stadiumPop = 0;
            // policeStationPop = 0;
            // fireStationPop = 0;
            // coalPowerPop = 0;
            // nuclearPowerPop = 0;
            // seaportPop = 0;
            // airportPop = 0;
            // needHospital = 0;
            // needChurch = 0;
            // crimeAverage = 0;
            // pollutionAverage = 0;
            // landValueAverage = 0;
            // cityTime = 0;
            // cityMonth = 0;
            // cityYear = 0;
            // startingYear = 0;
            // resHist10Max = 0;
            // resHist120Max = 0;
            // comHist10Max = 0;
            // comHist120Max = 0;
            // indHist10Max = 0;
            // indHist120Max = 0;
            // censusChanged = false;
            // roadSpend = 0;
            // policeSpend = 0;
            // fireSpend = 0;
            // roadFund = 0;
            // policeFund = 0;
            // fireFund = 0;
            // roadEffect = 0;
            // policeEffect = 0;
            // fireEffect = 0;
            // taxFund = 0;
            // cityTax = 0;
            // taxFlag = false;
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
            // roadPercent = 0.0f;
            // policePercent = 0.0f;
            // firePercent = 0.0f;
            // roadValue = 0;
            // policeValue = 0;
            // fireValue = 0;
            // mustDrawBudget = 0;
            // floodCount = 0;
            // cityYes = 0;
            // cityPop = 0;
            // cityPopDelta = 0;
            // cityAssessedValue = 0;
            // cityClass = CC_VILLAGE;
            // cityScore = 0;
            // cityScoreDelta = 0;
            // trafficAverage = 0;
            // terrainTreeLevel = -1;
            // terrainLakeLevel = -1;
            // terrainCurveLevel = -1;
            // terrainCreateIsland = -1;
            // graph10Max = 0;
            // graph120Max = 0;
            // simLoops = 0;
            // simPasses = 0;
            // simPass = 0;
            // simPaused = false;
            // simPausedSpeed = 3;
            // heatSteps = 0;
            // heatFlow = -7;
            // heatRule = 0;
            // heatWrap = 3;
            // cityFileName = "";
            // tilesAnimated = false;
            // doAnimation = true;
            // doMessages = true;
            // doNotices = true;
            // cellSrc
            // cellDst
            // cityPopLast = 0;
            // categoryLast = 0;
            // autoGoto = false;
            // powerStackPointer = 0;

            // more, more, more to come

            simInit();
        }

        public void destroy()
        {
            destroyMapArrays();
        }
    }
}
