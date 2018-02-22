namespace Micropolis.MicropolisCore
{
    /// <summary>
    /// Tool result.
    /// </summary>
    public enum ToolResult
    {
        TOOLRESULT_NO_MONEY = -2,  // User has not enough money for tool.
        TOOLRESULT_NEED_BULLDOZE = -1, // Clear the area first.
        TOOLRESULT_FAILED = 0, // Cannot build here.
        TOOLRESULT_OK = 1, // Build succeeded.
    }
}