using TMPro;
using UnityEngine;

namespace MicropolisGame
{
    public class GuiWindowFunds : MonoBehaviour
    {
        private TextMeshProUGUI _label;

        private void Awake()
        {
            _label = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        public void SetFunds(long funds)
        {
            _label.text = string.Format("Funds: {0:C0}", funds);
        }
    }
}