namespace MicropolisGame
{
    /// <summary>
    /// Enumeration of all the windows in the game. These
    /// have to match up to the name of the GameObjects 
    /// in the scene in order to register in GuiWindowManager startup.
    /// </summary>
    public enum EnumGuiWindow
    {
        MainMenu,
        Pause,
        Options,
        LoadCity,
        LoadScenario,
        LoadGraphics,
        Funds,
        Messages,
        Date,
        Population,
        InGameMenu
    }
}