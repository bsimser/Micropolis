using TMPro;
using UnityEngine;

namespace MicropolisGame
{
    public class GuiWindowDate : MonoBehaviour
    {
        private TextMeshProUGUI _label;

        private void Awake()
        {
            _label = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        public void SetDate(long year, long month)
        {
            _label.text = string.Format("{0} {1}", GetMonthName(month), year);
        }

        private static string GetMonthName(long month)
        {
            string[] monthNames =
            {
                "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            };
            return monthNames[month];
        }
    }
}