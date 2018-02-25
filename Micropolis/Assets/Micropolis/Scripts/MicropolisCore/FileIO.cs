using System;
using System.IO;

namespace MicropolisCore
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
            // If needed, construct a path to the file
            if (dir != null)
            {
                // TODO
            }

            var fileInfo = new FileInfo(filename);
            if (fileInfo.Length != 27120)
            {
                return false;
            }

            var result = true;
            using (var reader = new BinaryReader(File.OpenRead(filename)))
            {
                result = result &&
                    load_shorts(ref resHist, HISTORY_LENGTH / sizeof(short), reader) &&
                    load_shorts(ref comHist, HISTORY_LENGTH / sizeof(short), reader) &&
                    load_shorts(ref indHist, HISTORY_LENGTH / sizeof(short), reader) &&
                    load_shorts(ref crimeHist, HISTORY_LENGTH / sizeof(short), reader) &&
                    load_shorts(ref pollutionHist, HISTORY_LENGTH / sizeof(short), reader) &&
                    load_shorts(ref moneyHist, HISTORY_LENGTH / sizeof(short), reader) &&
                    load_shorts(ref miscHist, HISTORY_LENGTH / sizeof(short), reader);

                for (int x = 0; x < WORLD_W; x++)
                {
                    for (int y = 0; y < WORLD_H; y++)
                    {
                        var temp = map[x, y];
                        result = result && load_short(ref temp, reader);
                        map[x, y] = temp;
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Load an array of short values from file to memory.
        /// 
        /// Convert to the correct processor architecture, if necessary.
        /// </summary>
        /// <param name="buf">Buffer to put the loaded short values in.</param>
        /// <param name="len">Number of short values to load.</param>
        /// <param name="reader">File reader of the file to load from.</param>
        /// <returns>Load was succesfull.</returns>
        private bool load_shorts(ref short[] buf, int len, BinaryReader reader)
        {
            var result = true;
            for (int i = 0; i < len; i++)
            {
                result = result && load_short(ref buf[i], reader);
            }
            return result;
        }

        private bool load_short(ref short buf, BinaryReader reader)
        {
            var bytes = reader.ReadBytes(sizeof(short));
            if (bytes.Length != sizeof(short))
            {
                return false;
            }
            buf = swap_shorts(bytes);
            return true;
        }

        /// <summary>
        /// Swap upper and lower byte of all shorts in the array.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private short swap_shorts(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
                return BitConverter.ToInt16(bytes, 0);
            }
            return BitConverter.ToInt16(bytes, 0);
        }

        /// <summary>
        /// Load a file, and initialize the game variables.
        /// </summary>
        /// <param name="filename">Name of the file to load.</param>
        /// <returns>Load was succesfull.</returns>
        public bool loadFile(string filename)
        {
            if (!loadFileDir(filename, null))
            {
                return false;
            }

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
