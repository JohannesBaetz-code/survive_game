using UnityEngine;
using Utility;

namespace Entity.EntityState
{
    /// <summary>
    /// state of an entity / enemy when he is dead
    /// </summary>
    public class EntityDeathState : IEntityState
    {
        /// <summary>
        /// Is called the frame the entity dies.
        /// </summary>
        /// <param name="thisEntity"> The entity which has this state. </param>
        /// <returns> null (should probably called once per entity) </returns>
        public IEntityState UpdateState(Enemy thisEntity)
        {
            // ReferenceTable.Spawner.EnemyWave.Remove(thisEntity.gameObject);
            // ReferenceTable.HealthManager.RemoveHealthObject(thisEntity.gameObject);
            // GameObject.Destroy(thisEntity.gameObject);
            ReferenceTable.Spawner.DeSpawnEnemy(thisEntity.gameObject);
            return null;
        }
    }
}
