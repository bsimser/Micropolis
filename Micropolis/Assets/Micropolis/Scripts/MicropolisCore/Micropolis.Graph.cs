using System;

namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Copy history data to new array, scaling as needed.
        /// </summary>
        /// <param name="hist">Source history data.</param>
        /// <param name="s">Destination byte array.</param>
        /// <param name="scale">Scale factor.</param>
        public void drawMonth(short[] hist, char[] s, float scale)
        {
            int val, x;

            for (x = 0; x < 120; x++)
            {
                val = (int) (hist[x] * scale);
                s[119 - x] = (char) clamp(val, 0, 255);
            }
        }

        /// <summary>
        /// Set flag that graph data has been changed and graphs should be updated.
        /// </summary>
        public void changeCensus()
        {
            censusChanged = true;
        }

        /// <summary>
        /// If graph data has been changed, update all graphs.
        /// If graphs have been changed, tell the user front-end about it.
        /// </summary>
        public void graphDoer()
        {
            if (censusChanged)
            {
                callback("update", "s", "history");
                censusChanged = false;
            }
        }

        /// <summary>
        /// Initialize graphs
        /// </summary>
        public void initGraphs()
        {
            if (!historyInitialized)
            {
                historyInitialized = true;
            }
        }

        /// <summary>
        /// Compute various max ranges of graphs
        /// </summary>
        public void initGraphMax()
        {
            int x;

            resHist10Max = 0;
            comHist10Max = 0;
            indHist10Max = 0;

            for (x = 118; x >= 0; x--)
            {

                if (resHist[x] < 0)
                {
                    resHist[x] = 0;
                }
                if (comHist[x] < 0)
                {
                    comHist[x] = 0;
                }
                if (indHist[x] < 0)
                {
                    indHist[x] = 0;
                }

                resHist10Max = Math.Max(resHist10Max, resHist[x]);
                comHist10Max = Math.Max(comHist10Max, comHist[x]);
                indHist10Max = Math.Max(indHist10Max, indHist[x]);

            }

            graph10Max =
                Math.Max(resHist10Max,
                    Math.Max(comHist10Max,
                        indHist10Max));

            resHist120Max = 0;
            comHist120Max = 0;
            indHist120Max = 0;

            for (x = 238; x >= 120; x--)
            {

                if (resHist[x] < 0)
                {
                    resHist[x] = 0;
                }
                if (comHist[x] < 0)
                {
                    comHist[x] = 0;
                }
                if (indHist[x] < 0)
                {
                    indHist[x] = 0;
                }

                resHist120Max = Math.Max(resHist120Max, resHist[x]);
                comHist120Max = Math.Max(comHist120Max, comHist[x]);
                indHist120Max = Math.Max(indHist120Max, indHist[x]);

            }

            graph120Max =
                Math.Max(resHist120Max,
                    Math.Max(comHist120Max,
                        indHist120Max));
        }

        /// <summary>
        /// Get the minimal and maximal values of a historic graph.
        /// </summary>
        /// <param name="historyType">Type of history information. see HistoryType</param>
        /// <param name="historyScale">Scale of history data. see HistoryScale</param>
        /// <param name="minValResult">Pointer to variable to write minimal value to.</param>
        /// <param name="maxValResult">Pointer to variable to write maximal value to.</param>
        public void getHistoryRange(HistoryType historyType, HistoryScale historyScale, out short minValResult, out short maxValResult)
        {
            if (historyType < 0 || historyType >= HistoryType.HISTORY_TYPE_COUNT
                || historyScale < 0 || historyScale >= HistoryScale.HISTORY_SCALE_COUNT)
            {
                minValResult = 0;
                maxValResult = 0;
                return;
            }

            short[] history = null;
            switch (historyType)
            {
                case HistoryType.HISTORY_TYPE_RES:
                    history = resHist;
                    break;
                case HistoryType.HISTORY_TYPE_COM:
                    history = comHist;
                    break;
                case HistoryType.HISTORY_TYPE_IND:
                    history = indHist;
                    break;
                case HistoryType.HISTORY_TYPE_MONEY:
                    history = moneyHist;
                    break;
                case HistoryType.HISTORY_TYPE_CRIME:
                    history = crimeHist;
                    break;
                case HistoryType.HISTORY_TYPE_POLLUTION:
                    history = pollutionHist;
                    break;
                default:
                    break;
            }

            int offset = 0;
            switch (historyScale)
            {
                case HistoryScale.HISTORY_SCALE_SHORT:
                    offset = 0;
                    break;
                case HistoryScale.HISTORY_SCALE_LONG:
                    offset = 120;
                    break;
                default:
                    break;
            }

            short minVal = 32000;
            short maxVal = -32000;

            for (int i = 0; i < HISTORY_COUNT; i++)
            {
                short val = history[i + offset];

                minVal = Math.Min(val, minVal);
                maxVal = Math.Max(val, maxVal);
            }

            minValResult = minVal;
            maxValResult = maxVal;
        }

        /// <summary>
        /// Get a value from the history tables.
        /// </summary>
        /// <param name="historyType">Type of history information. see HistoryType</param>
        /// <param name="historyScale">Scale of history data. see HistoryScale</param>
        /// <param name="historyIndex">Index in the data to obtain</param>
        /// <returns>Historic data value of the requested graph</returns>
        public short getHistory(HistoryType historyType, HistoryScale historyScale, int historyIndex)
        {
            if (historyType < 0 || historyType >= HistoryType.HISTORY_TYPE_COUNT
                || historyScale < 0 || historyScale >= HistoryScale.HISTORY_SCALE_COUNT
                || historyIndex < 0 || historyIndex >= HISTORY_COUNT)
            {
                return 0;
            }

            short[] history = null;
            switch (historyType)
            {
                case HistoryType.HISTORY_TYPE_RES:
                    history = resHist;
                    break;
                case HistoryType.HISTORY_TYPE_COM:
                    history = comHist;
                    break;
                case HistoryType.HISTORY_TYPE_IND:
                    history = indHist;
                    break;
                case HistoryType.HISTORY_TYPE_MONEY:
                    history = moneyHist;
                    break;
                case HistoryType.HISTORY_TYPE_CRIME:
                    history = crimeHist;
                    break;
                case HistoryType.HISTORY_TYPE_POLLUTION:
                    history = pollutionHist;
                    break;
                default:
                    break;
            }

            int offset = 0;
            switch (historyScale)
            {
                case HistoryScale.HISTORY_SCALE_SHORT:
                    offset = 0;
                    break;
                case HistoryScale.HISTORY_SCALE_LONG:
                    offset = 120;
                    break;
                default:
                    break;
            }

            short result = history[historyIndex + offset];

            return result;
        }

        /// <summary>
        /// Store a value into the history tables.
        /// </summary>
        /// <param name="historyType">Type of history information. see HistoryType</param>
        /// <param name="historyScale">Scale of history data. see HistoryScale</param>
        /// <param name="historyIndex">Index in the data to obtain</param>
        /// <param name="historyValue">Index in the value to store</param>
        public void setHistory(HistoryType historyType, HistoryScale historyScale, int historyIndex, short historyValue)
        {
            if (historyType < 0 || historyType >= HistoryType.HISTORY_TYPE_COUNT
                || historyScale < 0 || historyScale >= HistoryScale.HISTORY_SCALE_COUNT
                || historyIndex < 0 || historyIndex >= HISTORY_COUNT)
            {
                return;
            }

            short[] history = null;
            switch (historyType)
            {
                case HistoryType.HISTORY_TYPE_RES:
                    history = resHist;
                    break;
                case HistoryType.HISTORY_TYPE_COM:
                    history = comHist;
                    break;
                case HistoryType.HISTORY_TYPE_IND:
                    history = indHist;
                    break;
                case HistoryType.HISTORY_TYPE_MONEY:
                    history = moneyHist;
                    break;
                case HistoryType.HISTORY_TYPE_CRIME:
                    history = crimeHist;
                    break;
                case HistoryType.HISTORY_TYPE_POLLUTION:
                    history = pollutionHist;
                    break;
                default:
                    break;
            }

            int offset = 0;
            switch (historyScale)
            {
                case HistoryScale.HISTORY_SCALE_SHORT:
                    offset = 0;
                    break;
                case HistoryScale.HISTORY_SCALE_LONG:
                    offset = 120;
                    break;
                default:
                    break;
            }

            history[historyIndex + offset] = historyValue;
        }
    }
}
