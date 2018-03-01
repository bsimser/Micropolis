using System;

namespace MicropolisCore
{
    public partial class Micropolis
    {
        private T clamp<T>(T val, T lower, T upper) where T : IComparable
        {
            if (val.CompareTo(lower) < 0)
            {
                return lower;
            }
            if (val.CompareTo(upper) > 0)
            {
                return upper;
            }
            return val;
        }

        /// <summary>
        /// Pause a simulation
        /// </summary>
        public void pause()
        {
            if (!simPaused)
            {
                simPausedSpeed = simSpeedMeta;
                setSpeed(0);
                simPaused = true;
            }

            // Call back even if the state did not change.
            callback("update", "s", "paused");
        }

        /// <summary>
        /// Resume simulation after pausing it
        /// </summary>
        public void resume()
        {
            if (simPaused)
            {
                simPaused = false;
                setSpeed((short) simPausedSpeed);
            }

            // Call back even if the state did not change.
            callback("update", "s", "paused");
        }

        public void setSpeed(short speed)
        {
            if (speed < 0)
            {
                speed = 0;
            }
            else if (speed > 3)
            {
                speed = 3;
            }

            simSpeedMeta = speed;

            if (simPaused)
            {
                simPausedSpeed = simSpeedMeta;
                speed = 0;
            }

            simSpeed = speed;

            callback("update", "s", "speed");
        }

        public void setPasses(int passes)
        {
            simPasses = passes;
            simPass = 0;
            callback("update", "s", "passes");
        }

        /// <summary>
        /// Set the game level and initial funds.
        /// </summary>
        /// <param name="level">New game level.</param>
        public void setGameLevelFunds(GameLevel level)
        {
            switch (level)
            {
                default:
                case GameLevel.LEVEL_EASY:
                    setFunds(20000);
                    setGameLevel(GameLevel.LEVEL_EASY);
                    break;

                case GameLevel.LEVEL_MEDIUM:
                    setFunds(10000);
                    setGameLevel(GameLevel.LEVEL_MEDIUM);
                    break;

                case GameLevel.LEVEL_HARD:
                    setFunds(5000);
                    setGameLevel(GameLevel.LEVEL_HARD);
                    break;
            }
        }

        /// <summary>
        /// Report to the front-end that a new game level has been set.
        /// </summary>
        public void updateGameLevel()
        {
            callback("update", "s", "gameLevel");
        }

        public void setCityName(string name)
        {
            string cleanName = string.Empty;

            int i;
            int n = name.Length;
            for (i = 0; i < n; i++)
            {
                char ch = name[i];
                if (!char.IsLetterOrDigit(ch))
                {
                    ch = '_';
                }
                cleanName += ch;
            }

            setCleanCityName(cleanName);
        }

        /// <summary>
        /// Set the name of the city.
        /// </summary>
        /// <param name="name">New name of the city</param>
        public void setCleanCityName(string name)
        {
            cityName = name;

            callback("update", "s", "cityName");
        }

        public void setYear(int year)
        {
            // Must prevent year from going negative, since it screws up the non-floored modulo arithmetic.
            if (year < startingYear)
            {
                year = startingYear;
            }

            year = (int) (year - startingYear - cityTime / 48);
            cityTime += year * 48;
            doTimeStuff();
        }

        /// <summary>
        /// Get the current year.
        /// </summary>
        /// <returns>The current game year</returns>
        public int currentYear()
        {
            return (int) (cityTime / 48 + startingYear);
        }

        /// <summary>
        /// Notify the user interface to start a new game.
        /// </summary>
        public void doNewGame()
        {
            callback("newGame", "");
        }

        /// <summary>
        /// set the enableDisasters flag, and set the flag to 
        /// update the user interface.
        /// </summary>
        /// <param name="value">New setting for enableDisasters</param>
        public void setEnableDisasters(bool value)
        {
            enableDisasters = value;
            mustUpdateOptions = true;
        }

        /// <summary>
        /// Set the auto-budget to the given value.
        /// </summary>
        /// <param name="value">New value for the auto-budget setting</param>
        public void setAutoBudget(bool value)
        {
            autoBudget = value;
            mustUpdateOptions = true;
        }

        /// <summary>
        /// Set the autoBulldoze flag to the given value,
        /// and set the mustUpdateOptions flag to update
        /// the user interface.
        /// </summary>
        /// <param name="value">The value to set autoBulldoze to</param>
        public void setAutoBulldoze(bool value)
        {
            autoBulldoze = value;
            mustUpdateOptions = true;
        }

        /// <summary>
        /// Set the autoGoto flag to the given value,
        /// and set the mustUpdateOptions flag to update
        /// the user interface.
        /// </summary>
        /// <param name="value">The value to set autoGoto to</param>
        public void setAutoGoto(bool value)
        {
            autoGoto = value;
            mustUpdateOptions = true;
        }

        /// <summary>
        /// Set the enableSound flag to the given value,
        /// and set the mustUpdateOptions flag to update
        /// the user interface.
        /// </summary>
        /// <param name="value">The value to set enableSound to</param>
        public void setEnableSound(bool value)
        {
            enableSound = value;
            mustUpdateOptions = true;
        }

        /// <summary>
        /// Set the doAnimation flag to the given value,
        /// and set the mustUpdateOptions flag to update
        /// the user interface.
        /// </summary>
        /// <param name="value">The value to set doAnimation to</param>
        public void setDoAnimation(bool value)
        {
            doAnimation = value;
            mustUpdateOptions = true;
        }

        /// <summary>
        /// Set the doMessages flag to the given value,
        /// and set the mustUpdateOptions flag to update
        /// the user interface.
        /// </summary>
        /// <param name="value">The value to set doMessages to</param>
        public void setDoMessages(bool value)
        {
            doMessages = value;
            mustUpdateOptions = true;
        }

        /// <summary>
        /// Set the doNotices flag to the given value,
        /// and set the mustUpdateOptions flag to update
        /// the user interface.
        /// </summary>
        /// <param name="value">The value to set doNotices to</param>
        public void setDoNotices(bool value)
        {
            doNotices = value;
            mustUpdateOptions = true;
        }

        /// <summary>
        /// Return the residential, commercial and industrial
        /// development demands, as floating point numbers
        /// from -1 (lowest demand) to 1 (highest demand).
        /// </summary>
        /// <param name="resDemandResult"></param>
        /// <param name="comDemandResult"></param>
        /// <param name="indDemandResult"></param>
        public void getDemands(out float resDemandResult, out float comDemandResult, out float indDemandResult)
        {
            resDemandResult = (float)resValve / (float)RES_VALVE_RANGE;
            comDemandResult = (float) comValve / (float) COM_VALVE_RANGE;
            indDemandResult = (float) indValve / (float) IND_VALVE_RANGE;
        }

        /// <summary>
        /// Set/change the game level.
        /// </summary>
        /// <param name="level">New game level.</param>
        void setGameLevel(GameLevel level)
        {
            //assert(level >= LEVEL_FIRST && level <= LEVEL_LAST);
            gameLevel = level;
            updateGameLevel();
        }
    }
}
