using System.IO;
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

            // TODO until we get a front end let's kick off the engine here with hard coded values
            // generate a random city
            //_engine.generateMap();
            // load a specific city
            _engine.loadFile("cities" + Path.DirectorySeparatorChar + "haight.cty");
            _engine.setSpeed(1);
            _engine.setCityTax(9);
            _engine.setEnableDisasters(false);
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