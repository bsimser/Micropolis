namespace Micropolis.MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Load a city file from a given filename and (optionally) directory.
        /// </summary>
        /// <param name="filename">Name of the file to load.</param>
        /// <param name="dir">f not NULL, name of the directory containing the file.</param>
        /// <returns>Load was succesfull.</returns>
        public bool loadFileDir(string filename, string dir)
        {
            return false;
        }

        /// <summary>
        /// Load a file, and initialize the game variables.
        /// </summary>
        /// <param name="filename">Name of the file to load.</param>
        /// <returns>Load was succesfull.</returns>
        public bool loadFile(string filename)
        {
            return false;
        }

        /// <summary>
        /// Save a game to disk.
        /// </summary>
        /// <param name="filename">Name of the file to use for storing the game.</param>
        /// <returns>The game was saved successfully.</returns>
        public bool saveFile(string filename)
        {
            return false;
        }

        /// <summary>
        /// Report to the front-end that the scenario was loaded.
        /// </summary>
        public void didLoadScenario()
        {
        }

        /// <summary>
        /// Try to load a new game from disk.
        /// TODO In what state is the game left when loading fails?
        /// TODO String normalization code is duplicated in Micropolis::saveCityAs(). Extract to a sub-function.
        /// </summary>
        /// <param name="filename">Name of the file to load.</param>
        /// <returns>Game was loaded successfully.</returns>
        public bool loadCity(string filename)
        {
            return false;
        }

        /// <summary>
        /// Report to the frontend that the game was successfully loaded.
        /// </summary>
        public void didLoadCity()
        {
        }

        /// <summary>
        /// Report to the frontend that the game failed to load.
        /// </summary>
        /// <param name="msg">File that attempted to load</param>
        public void didntLoadCity(string msg)
        {
        }

        /// <summary>
        /// Try to save the game.
        /// </summary>
        public void saveCity()
        {
        }

        /// <summary>
        /// Report to the frontend that the city is being saved.
        /// </summary>
        public void doSaveCityAs()
        {
        }

        /// <summary>
        /// Report to the frontend that the city was saved successfully.
        /// </summary>
        public void didSaveCity()
        {
        }

        /// <summary>
        /// Report to the frontend that the city could not be saved.
        /// </summary>
        /// <param name="msg">Name of the file used</param>
        public void didntSaveCity(string msg)
        {
        }

        /// <summary>
        /// Save the city under a new name (?)
        /// TODO String normalization code is duplicated in Micropolis::loadCity(). Extract to a sub-function.
        /// </summary>
        /// <param name="filename">Name of the file to use for storing the game.</param>
        public void saveCityAs(string filename)
        {
        }
    }
}
