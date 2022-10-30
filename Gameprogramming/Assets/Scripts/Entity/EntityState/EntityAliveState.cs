using UnityEngine;

namespace Entity.EntityState
{
    /// <summary>
    /// state of an entity / enemy when he is alive
    /// </summary>
    public class EntityAliveState : IEntityState
    {
        /// <summary>
        /// Is called every frame and checks if the entity is alive.
        /// </summary>
        /// <param name="thisEntity"> The entity which has this state. </param>
        /// <returns> the next State that will be executed. </returns>
        public IEntityState UpdateState(Enemy thisEntity)
        {
            if (thisEntity.CurrentHealth > 0)
            {
                return thisEntity.AliveState;
            }
            Debug.Log("Deathstate switch");
            return thisEntity.DeathState;
        }
    }
}
