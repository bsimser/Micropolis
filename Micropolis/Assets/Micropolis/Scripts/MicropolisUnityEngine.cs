namespace Micropolis
{
    public class MicropolisUnityEngine : MicropolisGenericEngine
    {
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