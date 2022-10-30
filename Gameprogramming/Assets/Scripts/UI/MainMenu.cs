using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    /// Script for main menu
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// Load level
        /// </summary>
        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        /// <summary>
        /// Exit game
        /// </summary>
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
