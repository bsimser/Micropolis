using MicropolisEngine;
using TMPro;
using UnityEngine;

namespace MicropolisGame
{
    public class UIManager
    {
        private readonly MicropolisUnityEngine _engine;
        private readonly TextMeshProUGUI _fundsText;
        private readonly TextMeshProUGUI _dateText;
        private readonly TextMeshProUGUI _poplationText;
        private readonly TextMeshProUGUI _messageText;

        public UIManager(MicropolisUnityEngine engine)
        {
            // assign the Micropois engine so our UI functions can use it
            _engine = engine;

            // find our UI elements in the scene
            _fundsText = GameObject.Find("Funds").GetComponent<TextMeshProUGUI>();
            _dateText = GameObject.Find("Date").GetComponent<TextMeshProUGUI>();
            _poplationText = GameObject.Find("Population").GetComponent<TextMeshProUGUI>();
            _messageText = GameObject.Find("Message").GetComponent<TextMeshProUGUI>();

            // subscribe to event handlers in the engine
            _engine.OnUpdateDate += DoUpdateDate;
            _engine.OnUpdateFunds += DoUpdateFunds;
            _engine.OnUpdateCityName += DoUpdateCityName;
            _engine.OnSendMessage += DoSendMessage;
        }

        private void DoSendMessage(short mesgNum, short s, short s1, bool picture, bool important)
        {
            //Debug.Log(string.Format("mesgNum: {0} x: {1} y: {2} picture: {3} important: {4}", mesgNum, s, s1, picture, important));
            //TODO handle autoGoto on important messages
            //TODO if there's a picture to show show it (how do we know what picture?)
            var message = _engine.messages[mesgNum];
            _messageText.text = message;
        }

        private void DoUpdateFunds()
        {
            _fundsText.text = string.Format("Funds: {0:C0}", _engine.totalFunds);
        }

        private void DoUpdateDate()
        {
            var year = _engine.cityYear;
            var month = _engine.cityMonth;
            _dateText.text = string.Format("{0} {1}", GetMonthName(month), year);

            // temporary until we find the message that triggers population update
            _poplationText.text = string.Format("Population: {0}", _engine.cityPop);
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