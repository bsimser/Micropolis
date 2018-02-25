using MicropolisEngine;
using UnityEngine;

namespace MicropolisGame
{
    public class GameManager : MonoBehaviour
    {
        private MicropolisUnityEngine _engine;
        private TileManager _tileManager;
        private UIManager _uiManager;

        private void Start()
        {
            // create an instance of the Micropolis engine
            _engine = MicropolisUnityEngine.CreateUnityEngine();

            // setup our internal manager classes
            _tileManager = new TileManager(_engine);
            _uiManager = new UIManager(_engine);

            // set the default values for a new city
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

            // TODO until we get a MicropolisPannedWindow let's kick off the engine here with hard coded values
            //_engine.setCityName("New City");
            _engine.generateMap();
            _engine.setSpeed(1);
            //_engine.doSimInit();
            //_engine.simPasses = 1;
            //_engine.setCityTax(9);
            _engine.setEnableDisasters(false);
            //_engine.setGameMode('start');
        }

        private void Update()
        {
            var ellapsedMilliseconds = (int) (Time.deltaTime * 1000);
            if (ellapsedMilliseconds % 16 == 0)
            {
                _engine.tickEngine();
                _tileManager.Draw();
            }
        }
    }
}