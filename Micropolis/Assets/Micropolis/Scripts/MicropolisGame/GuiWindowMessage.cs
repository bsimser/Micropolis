using TMPro;
using UnityEngine;

namespace MicropolisGame
{
    public class GuiWindowMessage : MonoBehaviour
    {
        private TextMeshProUGUI _label;

        private void Awake()
        {
            _label = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        public void SetMessage(string message)
        {
            _label.text = message;
        }
    }
}