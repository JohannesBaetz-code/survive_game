using System;
using System.Linq;
using UnityEngine;

namespace Entity.Spawner
{
    /// <summary>
    /// A spawnpoint, it disables itself if the player is nearby to not spawn an enemy in the player.
    /// </summary>
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Spawner _spawner;
        [SerializeField] private LayerMask _triggerLayer;

        /// <summary>
        /// remove the spawnpoint from the list of spawnpoints
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != _triggerLayer) return;
            _spawner.SpawnPoints.Remove(gameObject.transform);
        }

        /// <summary>
        /// add the spawnpoint to the spawnpointlist
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != _triggerLayer) return;
            _spawner.SpawnPoints.Add(gameObject.transform);
        }
    }
}
