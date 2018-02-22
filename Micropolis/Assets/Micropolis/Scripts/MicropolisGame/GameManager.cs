using MicropolisEngine;
using UnityEngine;

namespace MicropolisGame
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

            // TODO until we get a MicropolisPannedWindow let's kick off the engine here
            //_engine.title = "New City";
            //_engine.description = "A randomly generated city.";
            _engine.generateSomeCity(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
            //sendUpdate("load");
            _engine.setSpeed(2);
            _engine.setCityTax(9);
            _engine.setEnableDisasters(false);
            //_engine.setGameMode('start');
        }

        private void Update()
        {
            // fire the core engine every 50ms
            // TODO this timing is wrong but works for now
            var tick = (int) (Time.time * 1000 % _engine.cityTime) == 0;
            if (tick)
            {
                _engine.tickEngine();
            }
        }
    }
}