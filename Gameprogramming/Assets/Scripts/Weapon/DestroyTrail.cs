using System;
using System.Collections;
using UnityEngine;

namespace Weapon
{
    /// <summary>
    /// Destroy all parts of the bullet object if the particle effect itself is destroyed
    /// </summary>
    public class DestroyTrail : MonoBehaviour
    {
        [SerializeField] private GameObject[] _restBullet;
        [SerializeField] private float _destroyAfterTime;

        public Action DestroyAfterTime;

        private void Awake()
        {
            DestroyAfterTime += DestroyAll;
        }

        private void OnDisable()
        {
            DestroyAfterTime -= DestroyAll;
        }

        private void DestroyAll()
        {
            StartCoroutine(DestroyGameobject());
        }

        /// <summary>
        /// destroy all of the parent and child objects after short amount of time
        /// </summary>
        /// <returns></returns>
        private IEnumerator DestroyGameobject()
        {
            yield return new WaitForSeconds(_destroyAfterTime);
            foreach (var o in _restBullet)
            {
                Destroy(o);
            }
            Destroy(gameObject);
        }
    }
}
