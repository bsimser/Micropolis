using System;
using MicropolisEngine;
using UnityEngine;
using UnityEngine.UI;

namespace MicropolisGame
{
    public class GameManager : MonoBehaviour
    {
        private MicropolisUnityEngine _engine;

        // UI elements
        private Text _fundsText;
        private Text _dateText;

        private void Start()
        {
            _fundsText = GameObject.Find("Funds").GetComponent<Text>();
            _dateText = GameObject.Find("Date").GetComponent<Text>();

            _engine = MicropolisUnityEngine.CreateUnityEngine();

            // subscribe to event handlers in the engine
            _engine.OnUpdateDate += DoUpdateDate;
            _engine.OnUpdateFunds += DoUpdateFunds;
            _engine.OnUpdateCityName += DoUpdateCityName;

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
            _engine.setCityName("New City");
            _engine.generateSomeCity(UnityEngine.Random.Range(int.MinValue, int.MaxValue));
            _engine.setSpeed(1);
            //_engine.doSimInit();
            //_engine.simPasses = 1;
            //_engine.setCityTax(9);
            _engine.setEnableDisasters(false);
            //_engine.setGameMode('start');
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
                _engine.simTick();
                _engine.animateTiles();
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