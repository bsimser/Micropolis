namespace MicropolisCore
{
    /// <summary>
    /// Resource of the game (a file with data loaded in memory).
    /// </summary>
    public class Resource
    {
        private string buf; // Pointer to teh loaded file data
        private long size; // Size of the loaded file data
        private string name; // Name of the resource
        private long id; // Identification of the resource.
        private Resource next; // Pointer to next Resource
    }
}