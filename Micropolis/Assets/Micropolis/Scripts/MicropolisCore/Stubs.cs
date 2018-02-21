using System;
using UnityEngine;

namespace Micropolis.MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Deduct dollars from the player funds.
        /// </summary>
        /// <param name="dollars">Amount of money spent.</param>
        public void spend(int dollars)
        {
            setFunds((int) (totalFunds - dollars));
        }

        /// <summary>
        /// Set player funds to dollars.
        /// 
        /// Modify the player funds, and warn the front-end about the new amount of
        /// money.
        /// </summary>
        /// <param name="dollars">New value for the player funds.</param>
        public void setFunds(int dollars)
        {
            totalFunds = dollars;
            updateFunds();
        }

        /// <summary>
        /// Get number of ticks.
        /// </summary>
        /// <returns></returns>
        public long tickCount()
        {
            return DateTime.Now.Ticks;
        }

        public void doPlayNewCity()
        {
            callback("playNewCity", "");
        }

        public void doReallyStartGame()
        {
            callback("reallyStartGame", "");
        }

        public void doStartLoad()
        {
            callback("startLoad", "");
        }

        /// <summary>
        /// Tell the front-end a scenario is started.
        /// </summary>
        /// <param name="scenario">The scenario being started.</param>
        public void doStartScenario(int scenario)
        {
            callback("startScenario", "d", scenario.ToString());
        }

        /// <summary>
        /// Initialize the game.
        /// </summary>
        public void initGame()
        {
            simPaused = false; // Simulation is running
            simPausedSpeed = 0;
            simPass = 0;
            simPasses = 1;
            heatSteps = 0; // Disable cellular automata machine.
            setSpeed(0);
        }

        /// <summary>
        /// Scripting language independent callback mechanism.
        /// 
        /// This allows Micropolis to send callback messages with
        /// a variable number of typed parameters back to the
        /// scripting language, while maintining independence from
        /// the particular scripting language(or user interface
        /// runtime).
        /// 
        /// The name is the name of a message to send.
        /// The params is a string that specifies the number and
        /// types of the following vararg parameters.
        /// There is one character in the param string per vararg
        /// parameter. The following parameter types are currently
        /// supported:
        ///  - i: integer
        ///  - f: float
        ///  - s: string
        /// </summary>
        /// <param name="name">Name of the callback.</param>
        /// <param name="args">Parameters of the callback.</param>
        public void callback(string name, params string[] args)
        {
            Debug.Log(string.Format("callback:{0} arg:{1}", name, string.Join(", ", args)));
        }

        /// <summary>
        /// Tell the front-end to show an earthquake to the user (shaking the map for
        /// some time).
        /// </summary>
        /// <param name="strength"></param>
        public void doEarthquake(int strength)
        {
            //makeSound("city", "ExplosionLow"); // Make the sound all over.

            callback("startEarthquake", "d", strength.ToString());
        }

        /// <summary>
        /// Tell the front-end that the maps are not valid any more
        /// </summary>
        public void invalidateMaps()
        {
            mapSerial++;
            callback("update", "s", "map"); // new
        }

        /// <summary>
        /// Instruct the front-end to make a sound.
        /// </summary>
        /// <param name="channel">
        /// Name of the sound channel, which can effect the
        /// sound(location, volume, spatialization, etc).
        /// Use "city" for city sounds effects, and "interface"
        /// for user interface sounds.
        /// </param>
        /// <param name="sound">Name of the sound.</param>
        /// <param name="x">Tile X position of sound, 0 to WORLD_W, or -1 for everywhere.</param>
        /// <param name="y">Tile Y position of sound, 0 to WORLD_H, or -1 for everywhere.</param>
        public void makeSound(string channel, string sound, int x, int y)
        {
            if (enableSound)
            {
                //callback("makeSound", "ssdd", channel, sound, x, y);
            }
        }

        /// <summary>
        /// Get a tile from the map.
        /// </summary>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H.</param>
        /// <returns>Value of the map at the given position.</returns>
        public int getTile(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Set a tile into the map.
        /// </summary>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H.</param>
        /// <param name="tile">the tile value to set.</param>
        public void setTile(int x, int y, int tile)
        {
        }

        /// <summary>
        /// Get the address of the internal buffer containing the map. This is
        /// used to enable the tile engine to access the tiles directly.
        /// </summary>
        public void getMapBuffer()
        {
        }

        /// <summary>
        /// Get a value from the power grid map.
        /// </summary>
        /// <remarks>
        /// Off-map positions are considered to contain 0.
        /// </remarks>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H.</param>
        /// <returns>Value of the power grid map at the given position.</returns>
        public int getPowerGrid(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Set a value in the power grid map.
        /// </summary>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H.</param>
        /// <param name="power">the value to set.</param>
        public void setPowerGrid(int x, int y, int power)
        {
        }

        /// <summary>
        /// Get the address of the internal buffer containing the power grid
        /// map.This is used to enable the tile engine to access the power
        /// grid map directly.
        /// </summary>
        public void getPowerGridMapBuffer()
        {
        }

        /// <summary>
        /// Get a value from the population density map.
        /// </summary>
        /// <remarks>
        /// Off-map positions are considered to contain 0.
        /// </remarks>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W_2.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H_2.</param>
        /// <returns>Value of the population density map at the given position.</returns>
        public int getPopulationDensity(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Set a value in the population density map.
        /// </summary>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W_2.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H_2.</param>
        /// <param name="density">the value to set.</param>
        public void setPopulationDensity(int x, int y, int density)
        {
        }

        /// <summary>
        /// Get the address of the internal buffer containing the population
        /// density map.This is used to enable the tile engine to access the
        /// population density map directly.
        /// </summary>
        public void getPopulationDensityMapBuffer()
        {
        }

        /// <summary>
        /// Get a value from the rate of growth map.
        /// </summary>
        /// <remarks>
        /// Off-map positions are considered to contain 0.
        /// </remarks>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W_8.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H_8.</param>
        /// <returns>Value of the rate of growth map at the given position.</returns>
        public int getRateOfGrowth(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Set a value in the rate of growth map.
        /// </summary>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W_8.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H_8.</param>
        /// <param name="rate">the value to set.</param>
        public void setRateOfGrowth(int x, int y, int rate)
        {
        }

        /// <summary>
        /// Get the address of the internal buffer containing the rate of
        /// growth map.This is used to enable the tile engine to access the
        /// rate of growth map directly.
        /// </summary>
        public void getRateOfGrowthMapBuffer()
        {
        }

        /// <summary>
        /// Get a value from the traffic density map.
        /// TODO Use world coordinates instead (use trafficDensityMap.worldGet() instead).
        /// </summary>
        /// <remarks>
        /// Off-map positions are considered to contain 0.
        /// </remarks>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W_2.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H_2.</param>
        /// <returns>Value of the traffic density at the given position.</returns>
        public int getTrafficDensity(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Set a value in the traffic density map.
        /// TODO Use world coordinates instead (use trafficDensityMap.worldSet() instead).
        /// </summary>
        /// <remarks>
        /// Off-map positions are considered to contain 0.
        /// </remarks>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W_2.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H_2.</param>
        /// <param name="density">the value to set.</param>
        public void setTrafficDensity(int x, int y, int density)
        {
        }

        /// <summary>
        /// Get the address of the internal buffer containing the traffic
        /// density map.This is used to enable the tile engine to access the
        /// traffic density map directly.
        /// </summary>
        public void getTrafficDensityMapBuffer()
        {
        }

        // TODO more of the same 3 functions get, set, getbuffer for the following
        // TODO pollution density
        // TODO crime rate
        // TODO land value

        /// <summary>
        /// Get a value from the fire coverage map.
        /// TODO Use world coordinates instead (use fireStationEffectMap.worldGet() instead).
        /// </summary>
        /// <remarks>
        /// Off-map positions are considered to contain 0.
        /// </remarks>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W_8.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H_8.</param>
        /// <returns>Value of the fir coverage map at the given position.</returns>
        public int getFireCoverage(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Set a value in the fire coverage map.
        /// TODO Use world coordinates instead (use fireStationEffectMap.worldSet() instead).
        /// </summary>
        /// <remarks>
        /// Off-map positions are considered to contain 0.
        /// </remarks>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W_8.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H_8.</param>
        /// <param name="coverage">the value to set.</param>
        public void setFireCoverage(int x, int y, int coverage)
        {
        }

        /// <summary>
        /// Get the address of the internal buffer containing the fire coverage
        /// map.This is used to enable the tile engine to access the
        /// fire coverage map directly.
        /// </summary>
        public void getFireCoverageMapBuffer()
        {
        }

        /// <summary>
        /// Get a value from the police coverage map.
        /// TODO Use world coordinates instead (use policeStationEffectMap.worldGet() instead).
        /// </summary>
        /// <remarks>
        /// Off-map positions are considered to contain 0.
        /// </remarks>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W_8.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H_8.</param>
        /// <returns>Value of the fir coverage map at the given position.</returns>
        public int getPoliceCoverage(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Set a value in the police coverage map.
        /// TODO Use world coordinates instead (use policeStationEffectMap.worldSet() instead).
        /// </summary>
        /// <remarks>
        /// Off-map positions are considered to contain 0.
        /// </remarks>
        /// <param name="x">X coordinate of the position to get, 0 to WORLD_W_8.</param>
        /// <param name="y">Y coordinate of the position to get, 0 to WORLD_H_8.</param>
        /// <param name="coverage">the value to set.</param>
        public void setPoliceCoverage(int x, int y, int coverage)
        {
        }

        /// <summary>
        /// Get the address of the internal buffer containing the police coverage
        /// map.This is used to enable the tile engine to access the police 
        /// coverage map directly.
        /// </summary>
        public void getPoliceCoverageMapBuffer()
        {
        }
    }
}
