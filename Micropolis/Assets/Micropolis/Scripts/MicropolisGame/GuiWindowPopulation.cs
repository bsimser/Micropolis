using TMPro;
using UnityEngine;

namespace MicropolisGame
{
    public class GuiWindowPopulation : MonoBehaviour
    {
        private TextMeshProUGUI _label;

        private void Awake()
        {
            _label = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        public void SetPopulation(long population)
        {
            _label.text = string.Format("Population: {0}", population);
        }
    }
}