namespace Micropolis
{
    public class MicropolisUnityEngine : MicropolisGenericEngine
    {
        // list of messages to display
        // list of notices to show to user
        // list of scenarios the user can choose from

        public static MicropolisUnityEngine CreateUnityEngine()
        {
            var engine = new MicropolisUnityEngine();
            return engine;
        }

        public void tickEngine()
        {
            simTick();
            // TODO animateTiles();
            simUpdate();
        }
    }
}