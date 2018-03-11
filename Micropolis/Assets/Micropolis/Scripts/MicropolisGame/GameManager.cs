using System;
using System.IO;
using Cinemachine;
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

        public float zoomSpeed = 20f;

        [HideInInspector]
        public UIManager UIManager { get { return _uiManager; } }

        private void Start()
        {
            _uiManager = FindObjectOfType<UIManager>();
            _engine = MicropolisUnityEngine.CreateUnityEngine();
            _tileManager = new TileManager(_engine);
            _callbacks = new CallbackManager(this, _engine);

            InitZoom();
        }

        private void InitZoom()
        {
            // TODO find the Cinemachine camera
            var camera = FindObjectOfType<CinemachineVirtualCamera>();
            // TODO save the current zoom to our engine
            _engine.zoom = camera.m_Lens.OrthographicSize;
        }

        private void HandleZoom()
        {
            // TODO temporary function to handle zooming in and out
            // TODO move to an Input class or something later
            var zoom = _engine.zoom;
            // TODO use input mapping instead of hard coded key values here
            if (Input.GetKey(KeyCode.X) && zoom <= MicropolisUnityEngine.MIN_ZOOM ||
                Input.GetAxis("Mouse ScrollWheel") < 0 && zoom <= MicropolisUnityEngine.MIN_ZOOM)
            {
                // zoom out
                zoom += 1f * Time.deltaTime * zoomSpeed;
            }
            else if (Input.GetKey(KeyCode.Z) && zoom >= MicropolisUnityEngine.MAX_ZOOM ||
                     Input.GetAxis("Mouse ScrollWheel") > 0 && zoom >= MicropolisUnityEngine.MAX_ZOOM)
            {
                // zoom in
                zoom -= 1f * Time.deltaTime * zoomSpeed;
            }
            // if the zoom value changed enough then update the camera
            if (Math.Abs(zoom - _engine.zoom) > float.Epsilon)
            {
                var camera = FindObjectOfType<CinemachineVirtualCamera>();
                camera.m_Lens.OrthographicSize = zoom;
                _engine.zoom = zoom;
            }
        }

        private void Update()
        {
            // TODO probably should have a separate class to handle general input?
            if (Input.GetKeyDown(KeyCode.Escape) && !_engine.simPaused)
            {
                Pause();
            }

            HandleZoom();

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
            Debug.Log("dropdown: " + dropdown.itemText + " value: " + dropdown.value);
            var speed = dropdown.value;
            _engine.setSpeed((short) speed);
        }
    }
}