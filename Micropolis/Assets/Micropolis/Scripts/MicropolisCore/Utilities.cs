namespace Micropolis.MicropolisCore
{
    public partial class Micropolis
    {
        public void makeDollarDecimalStr(string numStr, string dollarStr)
        {
        }

        public void pause()
        {
        }

        public void resume()
        {
        }

        public void setSpeed(short speed)
        {
        }

        public void setPasses(int passes)
        {
        }

        public void updateGameLevel()
        {
        }

        public void setCityName(string name)
        {
        }

        /// <summary>
        /// Set the name of the city.
        /// </summary>
        /// <param name="name">New name of the city</param>
        public void setCleanCityName(string name)
        {
        }

        public void setYear(int year)
        {
        }

        /// <summary>
        /// Get the current year.
        /// </summary>
        /// <returns>The current game year</returns>
        public int currentYear()
        {
            return 0;
        }

        /// <summary>
        /// Notify the user interface to start a new game.
        /// </summary>
        public void doNewGame()
        {
        }

        /// <summary>
        /// set the enableDisasters flag, and set the flag to 
        /// update the user interface.
        /// </summary>
        /// <param name="value">New setting for enableDisasters</param>
        public void setEnableDisasters(bool value)
        {
        }

        /// <summary>
        /// Set the auto-budget to the given value.
        /// </summary>
        /// <param name="value">New value for the auto-budget setting</param>
        public void setAutoBudget(bool value)
        {
        }

        /// <summary>
        /// Set the autoBulldoze flag to the given value,
        /// and set the mustUpdateOptions flag to update
        /// the user interface.
        /// </summary>
        /// <param name="value">The value to set autoBulldoze to</param>
        public void setAutoBulldoze(bool value)
        {
        }

        /// <summary>
        /// Set the autoGoto flag to the given value,
        /// and set the mustUpdateOptions flag to update
        /// the user interface.
        /// </summary>
        /// <param name="value">The value to set autoGoto to</param>
        public void setAutoGoto(bool value)
        {
        }

        /// <summary>
        /// Set the enableSound flag to the given value,
        /// and set the mustUpdateOptions flag to update
        /// the user interface.
        /// </summary>
        /// <param name="value">The value to set enableSound to</param>
        public void setEnableSound(bool value)
        {
        }

        /// <summary>
        /// Set the doAnimation flag to the given value,
        /// and set the mustUpdateOptions flag to update
        /// the user interface.
        /// </summary>
        /// <param name="value">The value to set doAnimation to</param>
        public void setDoAnimation(bool value)
        {
        }

        /// <summary>
        /// Set the doMessages flag to the given value,
        /// and set the mustUpdateOptions flag to update
        /// the user interface.
        /// </summary>
        /// <param name="value">The value to set doMessages to</param>
        public void setDoMessages(bool value)
        {
        }

        /// <summary>
        /// Set the doNotices flag to the given value,
        /// and set the mustUpdateOptions flag to update
        /// the user interface.
        /// </summary>
        /// <param name="value">The value to set doNotices to</param>
        public void setDoNotices(bool value)
        {
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
            resDemandResult = 0f;
            comDemandResult = 0f;
            indDemandResult = 0f;
        }
    }
}
