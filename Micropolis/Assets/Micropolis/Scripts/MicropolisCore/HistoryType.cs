namespace MicropolisCore
{
    /// <summary>
    /// Available types of historic data.
    /// </summary>
    public enum HistoryType
    {
        HISTORY_TYPE_RES,   // Residiential history type
        HISTORY_TYPE_COM,   // Commercial history type
        HISTORY_TYPE_IND,   // Industry history type
        HISTORY_TYPE_MONEY, // Money history type
        HISTORY_TYPE_CRIME, // Crime history type
        HISTORY_TYPE_POLLUTION, // Pollution history type

        HISTORY_TYPE_COUNT,  // Number of history types
    }
}