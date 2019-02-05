using System.Collections.Generic;
using System.IO;
using MicropolisGame;
using TMPro;
using UnityEngine;

public class GuiWindowLoadCity : MonoBehaviour
{
    private List<string> _options;
    private TMP_Dropdown _dropdown;
    private GameManager _gameManager;

    private void Start()
    {
        // get a reference to the GameManager so we can launch the chosen city
        _gameManager = FindObjectOfType<GameManager>();

        // find the TextMeshPro dropdown component
        _dropdown = GetComponentInChildren<TMP_Dropdown>();

        // clear out the current options
        _dropdown.ClearOptions();

        // get a list of .cty files from the cities folder
        var files = Directory.GetFiles("cities", "*.cty");

        // initialize the options array of strings based on how many files there are
        _options = new List<string>(files.Length);

        foreach (var file in files)
        {
            // get the extended info about the file
            var fi = new FileInfo(file);

            // add the name to the options list
            _options.Add(fi.Name);
        }

        // set the dropdown options to be the new list
        _dropdown.AddOptions(_options);
    }

    /// <summary>
    /// Click event handler to fire when the user chooses a city and clicks "Load"
    /// </summary>
    public void OnLoadCityClick()
    {
        // get the index of the selected item
        var itemSelected = _dropdown.value;

        // get the city filename from the options array
        var cityFileName = _options[itemSelected];

        // load the city through the GameManager
        _gameManager.LoadCity(cityFileName);

        // close this window
        GuiWindowManager.Instance.Close(EnumGuiWindow.LoadCity);

        // turn on our game UI
        GuiWindowManager.Instance.ToggleGameElements(true);
    }
}