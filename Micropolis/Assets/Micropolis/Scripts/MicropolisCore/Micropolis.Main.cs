using System;
using System.IO;

namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Get version of Micropolis program.
        /// TODO Use this function or eliminate it.
        /// </summary>
        /// <returns>Textual version.</returns>
        public string getMicropolisVersion()
        {
            return "5.0";
        }

        /// <summary>
        /// Check whether dir points to a directory.
        /// </summary>
        /// <param name="dir">Directory to search.</param>
        /// <param name="envVar">Environment variable controlling searchpath of the directory.</param>
        /// <returns>Directory has been found.</returns>
        private bool testDirectory(string dir, string envVar)
        {
            if (Directory.Exists(dir))
            {
                return true;
            }

            //fprintf(stderr, "Can't find the directory \"%s\"!\n", dir.c_str());
            //fprintf(stderr, "The environment variable \"%s\" should name a directory.\n", envVar);

            return false;
        }

        /// <summary>
        /// Locate resource directory.
        /// </summary>
        public void environmentInit()
        {
            string s = Environment.GetEnvironmentVariable("SIMHOME");
            if (string.IsNullOrEmpty(s))
            {
                s = Directory.GetCurrentDirectory();
            }
            homeDir = s;

            if (testDirectory(homeDir, "$SIMHOME"))
            {
                resourceDir = homeDir + Path.PathSeparator + "res" + Path.PathSeparator;
                if (testDirectory(resourceDir, "$SIMHOME/res"))
                {
                    return; // All ok
                }
            }

            // Failed on $SIMHOME, ".", or the 'res' directory.
            //fprintf(stderr, "Please check the environment or reinstall Micropolis and try again! Sorry!\n");
            //exit(1);
        }

        /// <summary>
        /// Initialize for a simulation
        /// </summary>
        public void simInit()
        {
            setEnableSound(true); // Enable sound
            mustUpdateOptions = true; // Update options displayed at user
            scenario = ScenarioType.SC_NONE;
            startingYear = 1900;
            simPasses = 1;
            simPass = 0;
            setAutoGoto(true); // Enable auto-goto
            setCityTax(7);
            cityTime = 50;
            setEnableDisasters(true); // Enable disasters
            setAutoBulldoze(true); // Enable auto bulldoze
            setAutoBudget(true); // Enable auto-budget
            blinkFlag = 1;
            simSpeed = 3;
            changeEval();
            simPaused = false; // Simulation is running
            simLoops = 0;
            initSimLoad = 2;

            initMapArrays();
            initGraphs();
            initFundingLevel();
            resetMapState();
            resetEditorState();
            clearMap();
            initWillStuff();
            setFunds(5000);
            setGameLevelFunds(GameLevel.LEVEL_EASY);
            setSpeed(0);
            setPasses(1);
        }

        public void simUpdate()
        {
            blinkFlag = (short) (tickCount() % 60 < 30 ? 1 : -1);

            if (simSpeed != 0 && heatSteps == 0)
            {
                tilesAnimated = false;
            }

            doUpdateHeads();
            graphDoer();
            updateBudget();
            scoreDoer();
        }

        public void simHeat()
        {
            // TODO
        }

        public void simLoop(bool doSim)
        {
            if (heatSteps != 0)
            {
                for (int j = 0; j < heatSteps; j++)
                {
                    simHeat();
                }

                moveObjects();
                simRobots();

                newMap = 1;
            }
            else
            {
                if (doSim)
                {
                    simFrame();
                }

                moveObjects();
                simRobots();
            }

            simLoops++;
        }

        /// <summary>
        /// Move simulation forward.
        /// </summary>
        public void simTick()
        {
            if (simSpeed != 0)
            {
                for (simPass = 0; simPass < simPasses; simPass++)
                {
                    simLoop(true);
                }
            }
            simUpdate();
        }

        public void simRobots()
        {
            callback("simRobots", "");
        }
    }
}
