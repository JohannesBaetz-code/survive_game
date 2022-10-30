namespace Entity.EntityState
{
    /// <summary>
    /// Interface for State Pattern and entity states
    /// </summary>
    public interface IEntityState
    {
        /// <summary>
        /// Method that has to be implemented by every concrete state. Is called every frame and returns the next state.
        /// </summary>
        /// <param name="thisEntity"> the entity that has this state </param>
        /// <returns> the state that will be executed in the next frame. </returns>
        public IEntityState UpdateState(Enemy thisEntity);
    }
}
