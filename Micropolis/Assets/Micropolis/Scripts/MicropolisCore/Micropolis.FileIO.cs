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
                        var temp = (short) map[x, y];
                        result = result && load_short(ref temp, reader);
                        map[x, y] = (ushort) temp;
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
            int n;

            if (!loadFileDir(filename, null))
            {
                return false;
            }

            /* total funds is a long.....    miscHist is array of shorts */
            /* total funds is being put in the 50th & 51th word of miscHist */
            /* find the address, cast the ptr to a longPtr, take contents */

            n = half_swap_longs(miscHist, 50);
            setFunds(n);

            n = half_swap_longs(miscHist, 8);
            cityTime = n;

            /* yayaya */

            setAutoBulldoze(miscHist[52] != 0); // flag for autoBulldoze
            setAutoBudget(miscHist[53] != 0);   // flag for autoBudget
            setAutoGoto(miscHist[54] != 0);     // flag for auto-goto
            setEnableSound(miscHist[55] != 0);  // flag for the sound on/off
            setCityTax(miscHist[56]);
            setSpeed(miscHist[57]);
            changeCensus();
            mustUpdateOptions = true;

            policePercent = half_swap_longs(miscHist, 58) / 65536.0f;
            firePercent = half_swap_longs(miscHist, 60) / 65536.0f;
            roadPercent = half_swap_longs(miscHist, 62) / 65536.0f;

            cityTime = Math.Max(0, cityTime);

            // If the tax is nonsensical, set it to a reasonable value
            if (cityTax > 20 || cityTax < 0)
            {
                setCityTax(7);
            }

            // If the speed is nonsensical, set it to a reasonable value
            if (simSpeed < 0 || simSpeed > 3)
            {
                setSpeed(3);
            }

            setSpeed(simSpeed);
            setPasses(1);
            initFundingLevel();

            // Set the scenario id to 0.
            initWillStuff();
            // scenario = SC_NONE;
            initSimLoad = 1;
            doInitialEval = false;
            doSimInit();
            invalidateMaps();

            return true;
        }

        // this is called half_swap_longs but really does ints because in the current 
        // 64-bit architecture an int (4 bytes) is what a long was back then
        private int half_swap_longs(short[] shorts, int index)
        {
            var bytes = new byte[4];
            bytes[0] = BitConverter.GetBytes(shorts[index])[0];
            bytes[1] = BitConverter.GetBytes(shorts[index])[1];
            index++;
            bytes[2] = BitConverter.GetBytes(shorts[index])[0];
            bytes[3] = BitConverter.GetBytes(shorts[index])[1];
            if (BitConverter.IsLittleEndian)
            {
                bytes[2] = BitConverter.GetBytes(shorts[index])[0];
                bytes[3] = BitConverter.GetBytes(shorts[index])[1];
                index++;
                bytes[0] = BitConverter.GetBytes(shorts[index])[0];
                bytes[1] = BitConverter.GetBytes(shorts[index])[1];
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Save a game to disk.
        /// </summary>
        /// <param name="filename">Name of the file to use for storing the game.</param>
        /// <returns>The game was saved successfully.</returns>
        public bool saveFile(string filename)
        {
            // TODO
            return false;
        }

        /// <summary>
        /// Report to the front-end that the scenario was loaded.
        /// </summary>
        public void didLoadScenario()
        {
            callback("didLoadScenario", "");
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
            // TODO
            return false;
        }

        /// <summary>
        /// Report to the frontend that the game was successfully loaded.
        /// </summary>
        public void didLoadCity()
        {
            callback("didLoadCity", "");
        }

        /// <summary>
        /// Report to the frontend that the game failed to load.
        /// </summary>
        /// <param name="msg">File that attempted to load</param>
        public void didntLoadCity(string msg)
        {
            callback("didntLoadCity", "s", msg);
        }

        /// <summary>
        /// Try to save the game.
        /// </summary>
        public void saveCity()
        {
            // TODO
        }

        /// <summary>
        /// Report to the frontend that the city is being saved.
        /// </summary>
        public void doSaveCityAs()
        {
            callback("saveCityAs", "");
        }

        /// <summary>
        /// Report to the frontend that the city was saved successfully.
        /// </summary>
        public void didSaveCity()
        {
            callback("didSaveCity", "");
        }

        /// <summary>
        /// Report to the frontend that the city could not be saved.
        /// </summary>
        /// <param name="msg">Name of the file used</param>
        public void didntSaveCity(string msg)
        {
            callback("didntSaveCity", "s", msg);
        }

        /// <summary>
        /// Save the city under a new name (?)
        /// TODO String normalization code is duplicated in Micropolis::loadCity(). Extract to a sub-function.
        /// </summary>
        /// <param name="filename">Name of the file to use for storing the game.</param>
        public void saveCityAs(string filename)
        {
            // TODO
        }
    }
}
