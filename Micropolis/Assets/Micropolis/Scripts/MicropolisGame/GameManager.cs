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
        }

        private void Update()
        {
            var ellapsedMilliseconds = (int) (Time.deltaTime * 1000);
            if (ellapsedMilliseconds % 16 == 0)
            {
                // TODO should the check for speed be outside of this if?
                if (_engine.simSpeed != 0)
                {
                    _engine.tickEngine();
                    _tileManager.Draw();
                }
            }
        }

        // TODO maybe move all UI functions below to it's own class or the UI Manager?

        public void GenerateRandomCity()
        {
            _engine.initGame();
            _engine.simInit();
            _engine.generateMap();
            _engine.setSpeed(simSpeed);
            _engine.doSimInit();
            _engine.setEnableDisasters(false);
        }

        public void LoadCity()
        {
            _engine.initGame();
            _engine.simInit();
            _engine.loadCity("cities" + Path.DirectorySeparatorChar + "haight.cty");
            _engine.setSpeed(simSpeed);
            _engine.doSimInit();
            _engine.setEnableDisasters(false);
        }

        public void LoadScenario()
        {
            _engine.loadScenario(ScenarioType.SC_DULLSVILLE);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void Pause()
        {
            _engine.pause();
            // TODO show the pause panel
        }

        public void QuitToMain()
        {
            // TODO hide this panel
            // TODO end the simulator
            // TODO show main panel
        }

        public void Resume()
        {
            // TODO hide the pause panel
            _engine.resume();
        }

        public void LoadGraphics()
        {
            // TODO show a list of graphic tilesets
            // TODO reload the tileset with the new graphics
            // TODO resume the game (or just come back to the pause panel?)
        }
    }
}