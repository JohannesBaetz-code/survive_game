namespace PlayerController.PlayerState
{
    /// <summary>
    /// The Interface that has to be implemented by each playerstate.
    /// </summary>
    public interface IPlayerState
    {
        /// <summary>
        /// called every frame, evaluate current state
        /// </summary>
        /// <param name="player"> player object </param>
        /// <returns> state for next frame to evaluate </returns>
        public IPlayerState UpdatePlayerState(Player player);
    }
}
