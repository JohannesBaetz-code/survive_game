namespace PlayerController.PlayerState
{
    /// <summary>
    /// the state for the player which is alive
    /// </summary>
    public class PlayerAliveState : IPlayerState
    {
        /// <summary>
        /// evaluate every frame, checks if player is alive
        /// </summary>
        /// <param name="player"> the player object </param>
        /// <returns> the state which is evaluated next frame </returns>
        public IPlayerState UpdatePlayerState(Player player)
        {
            if (player.GetCurrentHealth() <= 0)
            {
                return new PlayerDeathState();
            }
            return player.AliveState;
        }
    }
}
