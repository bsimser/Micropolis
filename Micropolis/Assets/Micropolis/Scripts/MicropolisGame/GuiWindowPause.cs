using UnityEngine;

namespace MicropolisGame
{
    /// <inheritdoc />
    /// <summary>
    /// Handles all the interactions on the in-game pause screen
    /// </summary>
    public class GuiWindowPause : MonoBehaviour
    {
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        public void Resume()
        {
            GuiWindowManager.Instance.Close(EnumGuiWindow.Pause);
            _gameManager.Resume();
        }

        public void LoadGraphics()
        {
            GuiWindowManager.Instance.Open(EnumGuiWindow.LoadGraphics);        
        }

        public void Options()
        {
            // TODO need to add an option to the pause screen but we'll use this for now
            _gameManager.SaveCity();
        }

        public void Quit()
        {
            _gameManager.QuitToMain();
        }
    }
}