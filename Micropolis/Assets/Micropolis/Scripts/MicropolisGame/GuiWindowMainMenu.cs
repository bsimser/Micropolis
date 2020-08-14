using UnityEngine;

namespace MicropolisGame
{
    public class GuiWindowMainMenu : MonoBehaviour
    {
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        public void StartNewCity()
        {
            Debug.Log("StartNewCity ButtonClicked");
            _gameManager.StartNewCity();
            GuiWindowManager.Instance.Close(EnumGuiWindow.MainMenu);
            GuiWindowManager.Instance.ToggleGameElements(true);
        }

        public void LoadCity()
        {
            GuiWindowManager.Instance.Close(EnumGuiWindow.MainMenu);
            GuiWindowManager.Instance.Open(EnumGuiWindow.LoadCity);
        }

        public void LoadScenario()
        {
            GuiWindowManager.Instance.Close(EnumGuiWindow.MainMenu);
            GuiWindowManager.Instance.Open(EnumGuiWindow.LoadScenario);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}