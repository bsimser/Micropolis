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

            // TODO until we get a front end let's kick off the engine here with some hard coded values
            // generate a random city
            //_engine.generateMap();
            // load a sample city for testing
            _engine.loadFile("cities" + Path.DirectorySeparatorChar + "neatmap.cty");
            // other engine stuff
            _engine.setSpeed(1);
            _engine.setPasses(1);
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