using UnityEngine;

namespace Micropolis
{
    public class GameManager : MonoBehaviour
    {
        private MicropolisUnityEngine _engine;

        private void Start()
        {
            _engine = MicropolisUnityEngine.CreateUnityEngine();

            _engine.cityTax = 10;
            _engine.setPasses(200);
            // setTile = _engine.setTile;
            
            // addRobots

            //x = 0;
            //y = 0;
            
            //w = 800;
            //h = 600;
            
            // find the panel that is our MicropolisPannedWindow and set the engine
            // resize the panel
            // move the panel
            // show the panel
        }

        private void Update()
        {
            _engine.tickEngine();
        }
    }
}