namespace MicropolisCore
{
    /// <summary>
    /// Game levels
    /// </summary>
    public enum GameLevel
    {
        LEVEL_EASY,   // Simple game level
        LEVEL_MEDIUM, // Intermediate game level
        LEVEL_HARD,   // Difficult game level

        LEVEL_COUNT,  // Number of game levels

        LEVEL_FIRST = LEVEL_EASY, // First game level value
        LEVEL_LAST = LEVEL_HARD, // Last game level value
    }
}