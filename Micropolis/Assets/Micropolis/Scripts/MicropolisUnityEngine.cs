namespace Micropolis
{
    public class MicropolisUnityEngine : MicropolisGenericEngine
    {
        // list of messages to display
        // list of notices to show to user
        // list of scenarios the user can choose from

        public MicropolisUnityEngine()
        {
            initGameUnity();
        }

        public static MicropolisUnityEngine CreateUnityEngine()
        {
            var engine = new MicropolisUnityEngine();
            return engine;
        }

        public void tickEngine()
        {
            simTick();
            if (doAnimation && !tilesAnimated)
            {
                animateTiles();
            }
            simUpdate();
            // sendUpdate('tick')
            // sendUpdate('editor')
            // sendUpdate('map')
        }
    }
}