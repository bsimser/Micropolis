using System;
using System.IO;

namespace MicropolisCore
{
    public partial class Micropolis
    {
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
        /// Swap upper and lower words of all longs in the array.
        /// </summary>
        /// <param name="shorts"></param>
        /// <param name="index"></param>
        /// <returns></returns>
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

        private bool load_short(ref short buf, BinaryReader reader)
        {
            var bytes = reader.ReadBytes(sizeof(short));
            if (bytes.Length != sizeof(short))
            {
                return false;
            }

            buf = swap_shorts(bytes); // to intel

            return true;
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
                filename = dir + Path.DirectorySeparatorChar + filename;
            }

            if (!File.Exists(filename))
            {
                return false;
            }

            var fileInfo = new FileInfo(filename);
            const int size = 27120;
            var result = fileInfo.Length == size;

            if (!result)
            {
                return false;
            }

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
        /// Load a file, and initialize the game variables.
        /// </summary>
        /// <param name="filename">Name of the file to load.</param>
        /// <returns>Load was succesfull.</returns>
        private bool loadFile(string filename)
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
            scenario = ScenarioType.SC_NONE;
            initSimLoad = 1;
            doInitialEval = false;
            doSimInit();
            invalidateMaps();

            return true;
        }

        /// <summary>
        /// Save a game to disk.
        /// </summary>
        /// <param name="filename">Name of the file to use for storing the game.</param>
        /// <returns>The game was saved successfully.</returns>
        public bool saveFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return false;
            }

            /* total funds is a long.....    miscHist is array of ints */
            /* total funds is bien put in the 50th & 51th word of miscHist */
            /* find the address, cast the ptr to a longPtr, take contents */

            HALF_SWAP_LONGS(miscHist, 50, totalFunds);
            HALF_SWAP_LONGS(miscHist, 8, cityTime);

            miscHist[52] = (short) (autoBulldoze ? 1 : 0);
            miscHist[53] = (short) (autoBudget ? 1 : 0);
            miscHist[54] = (short) (autoGoto ? 1 : 0);
            miscHist[55] = (short) (enableSound ? 1 : 0);
            miscHist[57] = simSpeed;
            miscHist[56] = cityTax;

            /* yayaya */

            HALF_SWAP_LONGS(miscHist, 58, (long) (policePercent * 65536));
            HALF_SWAP_LONGS(miscHist, 60, (long) (firePercent * 65536));
            HALF_SWAP_LONGS(miscHist, 62, (long) (roadPercent * 65536));

            bool result = true;
            using (BinaryWriter f = new BinaryWriter(File.OpenWrite(filename)))
            {
                result =
                    save_short(resHist, HISTORY_LENGTH / 2, f) &&
                    save_short(comHist, HISTORY_LENGTH / 2, f) &&
                    save_short(indHist, HISTORY_LENGTH / 2, f) &&
                    save_short(crimeHist, HISTORY_LENGTH / 2, f) &&
                    save_short(pollutionHist, HISTORY_LENGTH / 2, f) &&
                    save_short(moneyHist, HISTORY_LENGTH / 2, f) &&
                    save_short(miscHist, HISTORY_LENGTH / 2, f);

                for (int x = 0; x < WORLD_W; x++)
                {
                    for (int y = 0; y < WORLD_H; y++)
                    {
                        result = result && save_short((short) map[x, y], f);
                    }
                }
            }

            return result;
        }

        private void HALF_SWAP_LONGS(short[] buf, int offset, long value)
        {
            var data = BitConverter.GetBytes(value);

            byte[] short1 = {data[0], data[1]};
            byte[] short2 = {data[2], data[4]};

            buf[offset] = BitConverter.ToInt16(short1, 0);
            buf[offset + 1] = BitConverter.ToInt16(short2, 0);
        }

        /// <summary>
        /// Save an array of short values from memory to file.
        /// 
        /// Convert to the correct endianness first, if necessary.
        /// </summary>
        /// <param name="buf">Buffer containing the short values to save.</param>
        /// <param name="len">Number of short values to save.</param>
        /// <param name="f">Stream of the file to save to.</param>
        /// <returns></returns>
        private bool save_short(short[] buf, int len, BinaryWriter f)
        {
            foreach (var s in buf)
            {
                save_short(s, f);
            }

            return true;
        }

        private bool save_short(short buf, BinaryWriter f)
        {
            var value = swap_shorts(BitConverter.GetBytes(buf));
            f.Write(value);
            return true;
        }

        /// <summary>
        /// Load a scenario.
        /// </summary>
        /// <remarks>
        /// s cannot be SC_NONE
        /// </remarks>
        /// <param name="s">Scenario to load.</param>
        public void loadScenario(ScenarioType s)
        {
            string name = null;
            string fname = null;

            cityFileName = string.Empty;

            setGameLevel(GameLevel.LEVEL_EASY);

            if (s < ScenarioType.SC_DULLSVILLE || s > ScenarioType.SC_RIO)
            {
                s = ScenarioType.SC_DULLSVILLE;
            }

            // TODO duplicates in part the List<Scenario> in MicropolisUnityEngine
            switch (s)
            {
                case ScenarioType.SC_DULLSVILLE:
                    name = "Dullsville";
                    fname = "snro.111";
                    scenario = ScenarioType.SC_DULLSVILLE;
                    cityTime = ((1900 - 1900) * 48) + 2;
                    setFunds(5000);
                    break;

                case ScenarioType.SC_SAN_FRANCISCO:
                    name = "San Francisco";
                    fname = "snro.222";
                    scenario = ScenarioType.SC_SAN_FRANCISCO;
                    cityTime = ((1906 - 1900) * 48) + 2;
                    setFunds(20000);
                    break;

                case ScenarioType.SC_HAMBURG:
                    name = "Hamburg";
                    fname = "snro.333";
                    scenario = ScenarioType.SC_HAMBURG;
                    cityTime = ((1965 - 1900) * 48) + 2;
                    setFunds(20000);
                    break;

                case ScenarioType.SC_BERN:
                    name = "Bern";
                    fname = "snro.444";
                    scenario = ScenarioType.SC_BERN;
                    cityTime = ((1965 - 1900) * 48) + 2;
                    setFunds(20000);
                    break;

                case ScenarioType.SC_TOKYO:
                    name = "Tokyo";
                    fname = "snro.555";
                    scenario = ScenarioType.SC_TOKYO;
                    cityTime = ((1957 - 1900) * 48) + 2;
                    setFunds(20000);
                    break;

                case ScenarioType.SC_DETROIT:
                    name = "Detroit";
                    fname = "snro.666";
                    scenario = ScenarioType.SC_DETROIT;
                    cityTime = ((1972 - 1900) * 48) + 2;
                    setFunds(20000);
                    break;

                case ScenarioType.SC_BOSTON:
                    name = "Boston";
                    fname = "snro.777";
                    scenario = ScenarioType.SC_BOSTON;
                    cityTime = ((2010 - 1900) * 48) + 2;
                    setFunds(20000);
                    break;

                case ScenarioType.SC_RIO:
                    name = "Rio de Janeiro";
                    fname = "snro.888";
                    scenario = ScenarioType.SC_RIO;
                    cityTime = ((2047 - 1900) * 48) + 2;
                    setFunds(20000);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("s", s, null);
            }

            setCleanCityName(name);
            setSpeed(3);
            setCityTax(7);

            loadFileDir(fname, resourceDir);

            initWillStuff();
            initFundingLevel();
            updateFunds();
            invalidateMaps();
            initSimLoad = 1;
            doInitialEval = true;
            doSimInit();
            didLoadScenario();
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
        /// TODO String normalization code is duplicated in Micropolis::saveCityAs(). 
        ///      Extract to a sub-function.
        /// </summary>
        /// <param name="filename">Name of the file to load.</param>
        /// <returns>Game was loaded successfully.</returns>
        public bool loadCity(string filename)
        {
            if (loadFile(filename))
            {
                cityFileName = filename;

                var lastSlash = cityFileName.LastIndexOf('/');
                var pos = (lastSlash == - 1) ? 0 : lastSlash + 1;

                var lastDot = cityFileName.LastIndexOf('.');
                var last = (lastDot == -1) ? cityFileName.Length : lastDot;

                var newCityName = cityFileName.Substring(pos, last - pos);
                setCityName(newCityName);

                didLoadCity();

                return true;
            }
            else
            {
                didntLoadCity(filename);
                return false;
            }
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
            // TEMP
            cityFileName = "cities/NEWCITY.CTY";
            using (var stream = File.Create(cityFileName))
            {
            }

            if (cityFileName.Length > 0)
            {
                // TODO doSaveCityAs();
                saveCityAs(cityFileName);
            }
            else
            {
                if (saveFile(cityFileName))
                {
                    didSaveCity();
                }
                else
                {
                    didntSaveCity(cityFileName);
                }
            }
        }

        /// <summary>
        /// Report to the frontend that the city is being saved.
        /// </summary>
        private void doSaveCityAs()
        {
            callback("saveCityAs", "");
        }

        /// <summary>
        /// Report to the frontend that the city was saved successfully.
        /// </summary>
        private void didSaveCity()
        {
            callback("didSaveCity", "");
        }

        /// <summary>
        /// Report to the frontend that the city could not be saved.
        /// </summary>
        /// <param name="msg">Name of the file used</param>
        private void didntSaveCity(string msg)
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
            cityFileName = filename;

            if (saveFile(cityFileName))
            {
                var lastDot = cityFileName.LastIndexOf(".");
                var lastSlash = cityFileName.LastIndexOf("/");

                var pos = lastSlash == -1 ? 0 : lastSlash + 1;
                var last = lastDot == -1 ? cityFileName.Length : lastDot;
                var len = last - pos;

                string newCityName = cityFileName.Substring(pos, len);

                setCityName(newCityName);

                didSaveCity();
            }
            else
            {
                didntSaveCity(cityFileName);
            }
        }
    }
}
