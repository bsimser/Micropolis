using System.IO;
using System.Linq;
using MicropolisEngine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MicropolisGame
{
    public class GameManager : MonoBehaviour
    {
        private MicropolisUnityEngine _engine;
        private TileManager _tileManager;
        private CallbackManager _callbacks;

        public short simSpeed;

        private void Awake()
        {
            _engine = MicropolisUnityEngine.CreateUnityEngine();
        }

        private void Start()
        {
            _tileManager = new TileManager(_engine);
            _callbacks = new CallbackManager(_engine);
        }

        private void Update()
        {
            // TODO probably should have a separate class to handle general input?
            if (Input.GetKeyDown(KeyCode.Escape) && !_engine.simPaused)
            {
                Pause();
            }

            // if the engine is running then let it update
            if (_engine.simSpeed != 0)
            {
                _engine.tickEngine();
                _tileManager.Draw();
            }
        }

        public void StartNewCity()
        {
            Debug.Log("Initing Game");
            _engine.initGame();
            Debug.Log("Initing Simulation");
            _engine.simInit();
            Debug.Log("Generating Map");
            _engine.generateMap();
            Debug.Log("Setting Speed");
            _engine.setSpeed(simSpeed);

            Debug.Log("Etcetera");
            _engine.doSimInit();
            _engine.setEnableDisasters(false);
        }

        public void LoadCity(string cityFileName)
        {
            _engine.initGame();
            _engine.simInit();
            _engine.loadCity("cities" + Path.DirectorySeparatorChar + cityFileName);
            _engine.setSpeed(simSpeed);
            _engine.doSimInit();
            _engine.setEnableDisasters(false);
        }

        public void LoadScenario(string scenarioFileName)
        {
            var scenario = _engine.scenarios.First(x => x.filename == scenarioFileName);
            _engine.loadScenario(scenario.id);
        }

        public float GetZoom()
        {
            return _engine.zoom;
        }

        public void SetZoom(float value)
        {
            _engine.zoom = value;
        }

        // TODO move to GuiWindowPause
        public void Pause()
        {
            _engine.pause();
            GuiWindowManager.Instance.Open(EnumGuiWindow.Pause);
        }

        public void QuitToMain()
        {
            GuiWindowManager.Instance.Close(EnumGuiWindow.Pause);
            // TODO end the simulator
            // TODO cleanup the map and other artifacts
            // TODO ask the user to save or save automatically?
            GuiWindowManager.Instance.Open(EnumGuiWindow.MainMenu);
        }

        public void Resume()
        {
            _engine.resume();
        }

        // TODO move to GuiWindowPause
        public void LoadGraphics()
        {
            // TODO show a list of graphic tilesets
            // TODO let the user pick from the list of tilesets
            // for now just randomly pick a tileset
            var tileset = _engine.tilesets[Random.Range(0, _engine.tilesets.Count)];
            _tileManager.Engine.LoadTileset(tileset.FolderName);
            Resume();
        }

        // TODO move to GuiWindowInGame
        public void SetSpeed(Dropdown dropdown)
        {
            var speed = dropdown.value;
            _engine.setSpeed((short)speed);
        }

        public void SaveCity()
        {
            _engine.saveCity();
        }
    }
}