using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MicropolisGame
{
    public class GuiWindowLoadGraphics : MonoBehaviour
    {
        private List<string> _options;
        private TMP_Dropdown _dropdown;
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _dropdown = GetComponentInChildren<TMP_Dropdown>();
            _dropdown.ClearOptions();
            // TODO add tilesets to options
            // TODO add _options to dropdown
        }

        public void OnLoadGraphicsClick()
        {
            var itemSelected = _dropdown.value;
            var tilesetName = _options[itemSelected];
            // TODO load the graphic tileset via the GameManager
            GuiWindowManager.Instance.Close(EnumGuiWindow.LoadGraphics);
        }
    }
}