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
        public void drawMonth(byte[] hist, byte[] s, float scale)
        {
            int val, x;

            for (x = 0; x < 120; x++)
            {
                val = (int) (hist[x] * scale);
                s[119 - x] = clamp<byte>((byte) val, 0, 255);
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
            // TODO
        }

        /// <summary>
        /// Get the minimal and maximal values of a historic graph.
        /// </summary>
        /// <param name="historyType">Type of history information. see HistoryType</param>
        /// <param name="historyScale">Scale of history data. see HistoryScale</param>
        /// <param name="minValResult">Pointer to variable to write minimal value to.</param>
        /// <param name="maxValResult">Pointer to variable to write maximal value to.</param>
        public void getHistoryRange(int historyType, int historyScale, out short minValResult, out short maxValResult)
        {
            // TODO
            minValResult = 0;
            maxValResult = 0;
        }

        /// <summary>
        /// Get a value from the history tables.
        /// </summary>
        /// <param name="historyType">Type of history information. see HistoryType</param>
        /// <param name="historyScale">Scale of history data. see HistoryScale</param>
        /// <param name="historyIndex">Index in the data to obtain</param>
        /// <returns>Historic data value of the requested graph</returns>
        public short getHistory(int historyType, int historyScale, int historyIndex)
        {
            // TODO
            return 0;
        }

        /// <summary>
        /// Store a value into the history tables.
        /// </summary>
        /// <param name="hisotryType">Type of history information. see HistoryType</param>
        /// <param name="historyScale">Scale of history data. see HistoryScale</param>
        /// <param name="historyIndex">Index in the data to obtain</param>
        /// <param name="historyValue">Index in the value to store</param>
        public void setHistory(int hisotryType, int historyScale, int historyIndex, short historyValue)
        {
            // TODO
        }
    }
}
