using MicropolisCore;
using MicropolisEngine;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace MicropolisGame
{
    public class GameManager : MonoBehaviour
    {
        private MicropolisUnityEngine _engine;
        private TileEngine _tileEngine;

        // UI elements
        private Text _fundsText;
        private Text _dateText;

        // TileMap to draw on
        private Tilemap _mapLayer;

        private void Start()
        {
            // Get UI elements
            _fundsText = GameObject.Find("Funds").GetComponent<Text>();
            _dateText = GameObject.Find("Date").GetComponent<Text>();

            // Get TileMap object
            _mapLayer = GameObject.Find("MapLayer").GetComponent<Tilemap>();

            // create an instance of the Micropolis engine
            _engine = MicropolisUnityEngine.CreateUnityEngine();
            _tileEngine = new TileEngine();

            // subscribe to event handlers in the engine
            _engine.OnUpdateDate += DoUpdateDate;
            _engine.OnUpdateFunds += DoUpdateFunds;
            _engine.OnUpdateCityName += DoUpdateCityName;

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

            // TODO initialize MapLayer tile map (if needed)
        }

        private void DoUpdateCityName()
        {
            Debug.Log(_engine.cityName);
        }

        private void DoUpdateFunds()
        {
            _fundsText.text = string.Format("Funds: {0:C0}", _engine.totalFunds);
        }

        private void DoUpdateDate()
        {
            var cityTime = _engine.cityTime;
            var startingYear = _engine.startingYear;
            var year = cityTime / 48 + startingYear;
            var month = cityTime % 48 >> 2;
            _dateText.text = string.Format("{0} {1}", GetMonthName(month), year);
        }

        private void Update()
        {
            var ellapsedMilliseconds = (int) (Time.deltaTime * 1000);
            if (ellapsedMilliseconds % 16 == 0)
            {
                _engine.tickEngine();


                // TODO update map
                UpdateMap();
            }
        }

        private void UpdateMap()
        {
            // TODO should we clear the layer on every pass?
            _mapLayer.ClearAllTiles();

            // TODO should only setup/update visible tiles
            for (int x = 0; x < Micropolis.WORLD_W; x++)
            {
                for (int y = 0; y < Micropolis.WORLD_H; y++)
                {
                    var tileId = _engine.map[x, y] & 1023;
                    _mapLayer.SetTile(new Vector3Int(x, y, 0), _tileEngine.GetTile(tileId));
                }
            }

        }

        private string GetMonthName(long month)
        {
            string[] monthNames =
            {
                "Jan",
                "Feb",
                "Mar",
                "Apr",
                "May",
                "Jun",
                "Jul",
                "Aug",
                "Sep",
                "Oct",
                "Nov",
                "Dec"
            };
            return monthNames[month];
        }
    }
}