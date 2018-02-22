namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Check progress of the user, and send him messages about it.
        /// </summary>
        public void sendMessages()
        {
        }

        /// <summary>
        /// Detect a change in city class, and produce a message if the player has
        /// reached the next class.
        /// </summary>
        public void checkGrowth()
        {
        }

        /// <summary>
        /// Send the user a message of an event that happens at a particular position
        /// in the city.
        /// </summary>
        /// <param name="msgNum">Message number of the message to display.</param>
        /// <param name="x">X coordinate of the position of the event.</param>
        /// <param name="y">Y coordinate of the position of the event.</param>
        /// <param name="picture">Flag that is true if a picture should be shown.</param>
        /// <param name="important">Flag that is true if the message is important.</param>
        public void sendMessage(short msgNum, short x, short y, bool picture, bool important)
        {
        }

        /// <summary>
        /// Make a sound for message mesgNum if appropriate.
        /// </summary>
        /// <param name="mesgNum">Message number displayed.</param>
        /// <param name="x">Horizontal coordinate in the city of the sound.</param>
        /// <param name="y">Vertical coordinate in the city of the sound.</param>
        public void doMakeSound(int mesgNum, int x, int y)
        {
        }

        /// <summary>
        /// Tell the front-end that it should perform an auto-goto
        /// </summary>
        /// <param name="x">X position at the map</param>
        /// <param name="y">Y position at the map</param>
        /// <param name="msg">Message</param>
        public void doAutoGoto(short x, short y, string msg)
        {
            callback("autoGoto", "dd", x.ToString(), y.ToString());
        }

        /// <summary>
        /// Tell the front-end that the player has lost the game
        /// </summary>
        public void doLoseGame()
        {
            callback("loseGame", "");
        }

        /// <summary>
        /// Tell the front-end that the player has won the game
        /// </summary>
        public void doWinGame()
        {
            callback("winGame", "");
        }
    }
}
