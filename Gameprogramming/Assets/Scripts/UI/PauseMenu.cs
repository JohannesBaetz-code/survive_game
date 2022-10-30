using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace UI
{
    /// <summary>
    /// Script for Pausemenu and Deathscreen
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        /// <summary>
        /// pause game
        /// </summary>
        private void OnEnable()
        {
            Time.timeScale = 0;
        }

        /// <summary>
        /// play game
        /// </summary>
        private void OnDisable()
        {
            Time.timeScale = 1;
            CursorManager.ExecuteCursorEvent(CursorManager.CursorEvent.Invisible);
        }

        public void BackToGame()
        {
            gameObject.SetActive(false);
            CursorManager.ExecuteCursorEvent(CursorManager.CursorEvent.Invisible);
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(0);
            CursorManager.ExecuteCursorEvent(CursorManager.CursorEvent.Invisible);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void RestartGame()
        {
            CursorManager.ExecuteCursorEvent(CursorManager.CursorEvent.Invisible);
            gameObject.SetActive(false);
            SceneManager.LoadScene(1);
        }
    }
}
