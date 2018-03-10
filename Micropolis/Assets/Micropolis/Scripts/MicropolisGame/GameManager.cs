using System.IO;
using MicropolisCore;
using MicropolisEngine;
using UnityEngine;

namespace MicropolisGame
{
    public class GameManager : MonoBehaviour
    {
        private MicropolisUnityEngine _engine;
        private TileManager _tileManager;
        private UIManager _uiManager;

        public short simSpeed;

        private void Start()
        {
            // create an instance of the Micropolis engine
            _engine = MicropolisUnityEngine.CreateUnityEngine();

            // setup our internal manager classes
            _tileManager = new TileManager(_engine);
            _uiManager = new UIManager(_engine);

            // TODO until we get a front end let's kick off the engine here with some hard coded values
            //GenerateRandomCity();
            LoadCity();
            //LoadScenario();
        }

        private void GenerateRandomCity()
        {
            _engine.initGame();
            _engine.simInit();
            _engine.generateMap();
            _engine.setSpeed(simSpeed);
            _engine.doSimInit();
            _engine.setEnableDisasters(false);
        }

        private void LoadCity()
        {
            _engine.initGame();
            _engine.simInit();
            _engine.loadCity("cities" + Path.DirectorySeparatorChar + "haight.cty");
            _engine.setSpeed(simSpeed);
            _engine.doSimInit();
            _engine.setEnableDisasters(false);
        }

        private void LoadScenario()
        {
            _engine.loadScenario(ScenarioType.SC_DULLSVILLE);
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