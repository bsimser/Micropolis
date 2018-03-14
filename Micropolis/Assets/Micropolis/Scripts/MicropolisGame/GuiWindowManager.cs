using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicropolisGame
{
    /// <inheritdoc />
    /// <summary>
    /// Main class to handle all the UI panels and text elements
    /// </summary>
    public class GuiWindowManager : Singleton<GuiWindowManager>
    {
        private readonly Dictionary<EnumGuiWindow, Transform> _windows = new Dictionary<EnumGuiWindow, Transform>();

        /// <summary>
        /// Public list of all UI elements that we can show/hide
        /// </summary>
        public Transform[] Windows;

        private void Start()
        {
            // register all UI windows so we can reference them through the EnumGuiWindow type
            foreach (var window in Windows)
            {
                _windows[(EnumGuiWindow) Enum.Parse(typeof(EnumGuiWindow), window.name)] = window;
            }

            // setup the initial view we want to start with
            CloseAll();
            ToggleGameElements(false);
            Open(EnumGuiWindow.MainMenu);
        }

        public Transform GetWindow(EnumGuiWindow window)
        {
            return _windows.ContainsKey(window) ? _windows[window] : null;
        }

        public void Open(EnumGuiWindow window)
        {
            Show(window, true);
        }

        public void Close(EnumGuiWindow window)
        {
            Show(window, false);
        }

        public void CloseAll()
        {
            foreach (var window in _windows)
            {
                Close(window.Key);
            }
        }

        private void Show(EnumGuiWindow window, bool enable)
        {
            var windowTransform = _windows.ContainsKey(window) ? _windows[window] : null;
            if (windowTransform != null)
            {
                windowTransform.gameObject.SetActive(enable);
            }
        }

        public void ToggleGameElements(bool show)
        {
            Show(EnumGuiWindow.Funds, show);
            Show(EnumGuiWindow.Messages, show);
            Show(EnumGuiWindow.Date, show);
            Show(EnumGuiWindow.Population, show);
            Show(EnumGuiWindow.InGameMenu, show);
        }
    }
}