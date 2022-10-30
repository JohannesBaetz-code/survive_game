using System;
using System.Collections;
using UnityEngine;

namespace Weapon
{
    /// <summary>
    /// Script to destroy the trail of the particle effect
    /// </summary>
    public class DestroyParticle : MonoBehaviour
    {
        [SerializeField] private DestroyTrail _trail;

        private void OnDestroy()
        {
            _trail.DestroyAfterTime?.Invoke();
        }
    }
}
