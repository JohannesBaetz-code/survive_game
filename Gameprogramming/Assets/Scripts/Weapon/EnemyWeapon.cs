using System;
using Playercontroller.Shooting;
using UnityEngine;

namespace Weapon
{
    /// <summary>
    /// Enemyweapon which cannot be used by a player, its only created for enemies with fewer variables and methods
    /// </summary>
    [Serializable]
    public class EnemyWeapon : UniversalWeapon
    {
        [SerializeField] private int _maxBurst;
        [SerializeField] protected float _burstDelay;
        
        private WeaponInformation _weaponInformation;
        private int _currentRound;
        public Vector3 DirectionToPlayer { get; set; }

        public void Awake(ref WeaponInformation weaponInformation)
        {
            base.Awake();
            _weaponInformation = weaponInformation;
            _currentRound = 0;
        }

        /// <summary>
        /// Shoot after every delay
        /// </summary>
        public override void OnShoot()
        {
            if (DeltaTimeForShot(Time.time) && CanShootInRound())
            {
                _canShoot = true;
            }
        }

        /// <summary>
        /// shoot raycast to player
        /// </summary>
        /// <param name="rayDirection"> direction of raycast </param>
        /// <returns> true if this frame was shot, false if this frame wasn't shot </returns>
        public override bool TryUpdateAndShoot(out Vector3 rayDirection)
        {
            rayDirection = Vector3.zero;
            if (_canShoot && HasAmmoInClip())
            {
                TryHandleRayCastShot(out rayDirection);
                _canShoot = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// reload the weapon (not implemented)
        /// </summary>
        public override void Reload()
        {
        }

        /// <summary>
        /// implementation of raycast shot, event random offset added, so some shots could miss the player
        /// </summary>
        /// <param name="rayDirection"> direction of the raycast </param>
        /// <returns> true if the player was hit, false if not </returns>
        protected bool TryHandleRayCastShot(out Vector3 rayDirection)
        {
            Vector3 random = RandomShot();
            Ray ray = new Ray();
            ray.direction = DirectionToPlayer - _weaponInformation.ShootStartPoint.position + random;
            ray.origin = _weaponInformation.ShootStartPoint.position;

            rayDirection = ray.direction;
            _lastShot = Time.time;

            if (Physics.Raycast(ray, out RaycastHit hit, _range))
            {
                _weaponInformation.HealthManager.AdjustHealth(hit.collider.gameObject, -_damage);
                return true;
            }

            return false;
        }

        /// <summary>
        /// calculate the time until the next shot, also shots get delay's to simulate a burst fire and not just infinite rapid fire
        /// </summary>
        /// <returns> true if a shot is possible </returns>
        private bool CanShootInRound()
        {
            if (_currentRound < _maxBurst)
            {
                _currentRound++;
                return true;
            }
            else
            {
                _currentRound = 0;
                _lastShot = Time.time + _burstDelay;
                return false;
            }
        }
    }
}
