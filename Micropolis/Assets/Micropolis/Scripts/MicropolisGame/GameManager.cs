using System.IO;
using MicropolisCore;
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
        private UIManager _uiManager;

        public short simSpeed;

        [HideInInspector]
        public UIManager UIManager { get { return _uiManager; } }

        [HideInInspector]
        public MicropolisUnityEngine Engine { get { return _engine; } }

        private void Awake()
        {
            _engine = MicropolisUnityEngine.CreateUnityEngine();
        }

        private void Start()
        {
            _uiManager = FindObjectOfType<UIManager>();
            _tileManager = new TileManager(_engine);
            _callbacks = new CallbackManager(this, _engine);
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

        public void GenerateRandomCity()
        {
            _uiManager.HideMainPanel();
            _engine.initGame();
            _engine.simInit();
            _engine.generateMap();
            _engine.setSpeed(simSpeed);
            _engine.doSimInit();
            _engine.setEnableDisasters(false);
            _uiManager.ToggleGameElements(true);
        }

        public void LoadCity()
        {
            _uiManager.HideMainPanel();
            _engine.initGame();
            _engine.simInit();
            _engine.loadCity("cities" + Path.DirectorySeparatorChar + "haight.cty");
            _engine.setSpeed(simSpeed);
            _engine.doSimInit();
            _engine.setEnableDisasters(false);
            _uiManager.ToggleGameElements(true);
        }

        public void LoadScenario()
        {
            _uiManager.HideMainPanel();
            _engine.loadScenario(ScenarioType.SC_DULLSVILLE);
            _uiManager.ToggleGameElements(true);
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
            _uiManager.ShowPausePanel();
        }

        public void QuitToMain()
        {
            _uiManager.HidePausePanel();
            // TODO end the simulator
            _uiManager.ShowMainPanel();
        }

        public void Resume()
        {
            _uiManager.HidePausePanel();
            _engine.resume();
        }

        public void LoadGraphics()
        {
            // TODO show a list of graphic tilesets
            // TODO let the user pick from the list of tilesets
            // for now just randomly pick a tileset
            var tileset = _engine.tilesets[Random.Range(0, _engine.tilesets.Count)];
            _tileManager.Engine.LoadTileset(tileset.FolderName);
            Resume();
        }

        public void SetSpeed(Dropdown dropdown)
        {
            var speed = dropdown.value;
            _engine.setSpeed((short)speed);
        }
    }
}