using MicropolisEngine;
using UnityEngine;
using UnityEngine.UI;

namespace MicropolisGame
{
    public class UIManager
    {
        private readonly MicropolisUnityEngine _engine;
        private readonly Text _fundsText;
        private readonly Text _dateText;

        public UIManager(MicropolisUnityEngine engine)
        {
            // assign the Micropois engine so our UI functions can use it
            _engine = engine;

            // find our UI elements in the scene
            _fundsText = GameObject.Find("Funds").GetComponent<Text>();
            _dateText = GameObject.Find("Date").GetComponent<Text>();

            // subscribe to event handlers in the engine
            _engine.OnUpdateDate += DoUpdateDate;
            _engine.OnUpdateFunds += DoUpdateFunds;
            _engine.OnUpdateCityName += DoUpdateCityName;
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

        private void DoUpdateCityName()
        {
            Debug.Log(_engine.cityName);
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