namespace Micropolis.MicropolisCore
{
    /// <summary>
    /// Get resources (from files)
    /// </summary>
    public partial class Micropolis
    {
        /// <summary>
        /// Get the text of a message.
        /// </summary>
        /// <param name="str">Destination of the text (usually 256 characters long)</param>
        /// <param name="id">Identification of the resource</param>
        /// <param name="num">Number of the string in the resource</param>
        public void getIndString(string str, int id, short num)
        {
        }
    }

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