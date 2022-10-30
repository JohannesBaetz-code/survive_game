using UnityEngine.SceneManagement;

namespace PlayerController.PlayerState
{
    /// <summary>
    /// the state for the player which has died
    /// </summary>
    public class PlayerDeathState : IPlayerState
    {
        /// <summary>
        /// evaluate every frame, enables the deathscreen when its disabled
        /// </summary>
        /// <param name="player"> the player object </param>
        /// <returns> the state which is evaluated next frame </returns>
        public IPlayerState UpdatePlayerState(Player player)
        {
            SceneManager.LoadScene(2);
            return null;
        }
    }
}
