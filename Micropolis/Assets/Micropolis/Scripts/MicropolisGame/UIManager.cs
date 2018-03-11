using TMPro;
using UnityEngine;

namespace MicropolisGame
{
    /// <inheritdoc />
    /// <summary>
    /// Main class to handle all the UI panels and text elements
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public Transform mainPanel;
        public Transform pausePanel;
        public Transform optionsPanel;
        public Transform fundsPanel;
        public Transform messagePanel;
        public Transform datePanel;
        public Transform populationPanel;

        private TextMeshProUGUI _fundsText;
        private TextMeshProUGUI _dateText;
        private TextMeshProUGUI _populationText;
        private TextMeshProUGUI _messageText;

        // TODO HideMainPanel
        // TODO ShowOptionsPanel
        // TODO HideOptionsPanel
        // TODO ShowLoadGraphicsPanel
        // TODO HideLoadGraphicsPanel

        private void Start()
        {
            // bind all of our text elements from our panels (could change if UI changes)
            _fundsText = fundsPanel.GetComponent<TextMeshProUGUI>();
            _dateText = datePanel.GetComponent<TextMeshProUGUI>();
            _populationText = populationPanel.GetComponent<TextMeshProUGUI>();
            _messageText = messagePanel.GetComponent<TextMeshProUGUI>();

            // setup the initial view we want to start with
            HidePausePanel();
            ToggleTextElements(false);
            ShowMainPanel();
        }

        public void ToggleTextElements(bool show)
        {
            fundsPanel.gameObject.SetActive(show);
            messagePanel.gameObject.SetActive(show);
            datePanel.gameObject.SetActive(show);
            populationPanel.gameObject.SetActive(show);
        }

        public void SetFunds(long funds)
        {
            _fundsText.text = string.Format("Funds: {0:C0}", funds);
        }

        public void SetDate(long year, long month)
        {
            _dateText.text = string.Format("{0} {1}", GetMonthName(month), year);
        }

        public void SetPopulation(long population)
        {
            _populationText.text = string.Format("Population: {0}", population);
        }

        public void SetMessage(string message)
        {
            _messageText.text = message;
        }

        private static string GetMonthName(long month)
        {
            string[] monthNames =
            {
                "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            };
            return monthNames[month];
        }

        public void ShowPausePanel()
        {
            pausePanel.gameObject.SetActive(true);
        }

        public void HidePausePanel()
        {
            pausePanel.gameObject.SetActive(false);
        }

        public void ShowMainPanel()
        {
            mainPanel.gameObject.SetActive(true);
        }

        public void HideMainPanel()
        {
            mainPanel.gameObject.SetActive(false);
        }
    }
}