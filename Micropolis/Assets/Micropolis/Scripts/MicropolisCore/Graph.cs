namespace Micropolis.MicropolisCore
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
        }

        /// <summary>
        /// Set flag that graph data has been changed and graphs should be updated.
        /// </summary>
        public void changeCensus()
        {
        }

        /// <summary>
        /// If graph data has been changed, update all graphs.
        /// If graphs have been changed, tell the user front-end about it.
        /// </summary>
        public void graphDoer()
        {
        }

        /// <summary>
        /// Initialize graphs
        /// </summary>
        public void initGraphs()
        {
        }

        /// <summary>
        /// Compute various max ranges of graphs
        /// </summary>
        public void initGraphMax()
        {
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
        }
    }
}
