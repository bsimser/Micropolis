using System.Collections.Generic;
using System.IO;
using MicropolisGame;
using TMPro;
using UnityEngine;

public class GuiWindowLoadScenario : MonoBehaviour
{
    private List<string> _options;
    private TMP_Dropdown _dropdown;
    private GameManager _gameManager;

    private void Start()
    {
        // get a reference to the GameManager so we can launch the chosen scenario
        _gameManager = FindObjectOfType<GameManager>();

        // find the TextMeshPro dropdown component
        _dropdown = GetComponentInChildren<TMP_Dropdown>();

        // clear out the current options
        _dropdown.ClearOptions();

        // get a list of scenario files from the res folder
        var files = Directory.GetFiles("res", "snro.*");

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

    public void OnLoadScenarioClick()
    {
        // get the index of the selected item
        var itemSelected = _dropdown.value;

        // get the scenario from the options array
        var scenarioFileName = _options[itemSelected];

        // load the scenario via the GameManager
        _gameManager.LoadScenario(scenarioFileName);

        // close this window
        GuiWindowManager.Instance.Close(EnumGuiWindow.LoadScenario);

        // enable the game UI elements
        GuiWindowManager.Instance.ToggleGameElements(true);
    }
}