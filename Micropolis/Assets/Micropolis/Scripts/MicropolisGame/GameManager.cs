using System;
using MicropolisCore;
using MicropolisEngine;
using UnityEngine;
using UnityEngine.UI;

namespace MicropolisGame
{
    public class GameManager : MonoBehaviour
    {
        private MicropolisUnityEngine _engine;
        private const int TicksPerMicrosecond = 10;

        private double _lastTick;

        // UI elements
        private Text _fundsText;
        private Text _dateText;

        private void Start()
        {
            _fundsText = GameObject.Find("Funds").GetComponent<Text>();
            _dateText = GameObject.Find("Date").GetComponent<Text>();

            _engine = MicropolisUnityEngine.CreateUnityEngine();

            // subscribe to event handlers in the engine
            _engine.OnDateUpdated += UpdateDate;

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

            /*
            GameSpeeds = [ 
            # Speed, Passes, Label
            (0, 0, 'Paused',),
            (1, 1, 'Ultra Slow',),
            (2, 1, 'Very Slow',),
            (3, 1, 'Slow',),
            (3, 5, 'Medium',),
            (3, 10, 'Fast',),
            (3, 50, 'Very Fast',),
            (3, 100, 'Ultra Fast',),
            (3, 500, 'Ridiculously Fast',),
            (3, 1000, 'Outrageously Fast',),
            (3, 5000, 'Astronomically Fast',),
                ]
            */
            _engine.setSpeed(3);
            _engine.simPasses = 1;

            _engine.setCityTax(9);
            _engine.setEnableDisasters(false);
            //_engine.setGameMode('start');
        }

        private void UpdateDate()
        {
            var cityTime = _engine.cityTime;
            var startingYear = _engine.startingYear;
            var year = cityTime / 48 + startingYear;
            var month = cityTime % 48 >> 2;
            _dateText.text = string.Format("{0} {1}", GetMonthName(month), year);
            //Debug.Log("cityTime=" + cityTime + " date=" + _dateText.text);
        }

        private float _nextActionTime = 0.0f;
        private float _period = 1.5f;

        private void Update()
        {
            // orginal game called tickEngine every 50 microseconds

            // TODO can't get speed right
            // original DOS version slow = 1 month = 20 seconds
            //                      medium = 1 month = 5 seconds
            //                      fast = 1 month = 2.5 seconds
            // simcity for windows  1 year = 210 seconds on slow

            var microseconds = Math.Floor((DateTime.Now.Ticks % TimeSpan.TicksPerMillisecond) / (double) TicksPerMicrosecond);
            var simDelay = microseconds * 50; // * Micropolis.PASSES_PER_CITYTIME;

            //Debug.Log("ms=" + microseconds + " lasttick=" + _lastTick);
            if (Time.time > _nextActionTime)
            {
                _nextActionTime += _period;
                _engine.tickEngine();
            }

            if (_lastTick > simDelay)
            {
                //Debug.Log("ms=" + microseconds + " lasttick=" + _lastTick);
                _lastTick = 0;
            }
            else
            {
                _lastTick += microseconds;
            }

            // TODO setup event when funds change
            _fundsText.text = string.Format("Funds: {0:C0}", _engine.totalFunds);
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