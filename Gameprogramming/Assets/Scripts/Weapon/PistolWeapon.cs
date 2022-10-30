using System;
using Playercontroller.Shooting;
using UnityEngine;

namespace Weapon
{
    /// <summary>
    /// Implementation of the pistol
    /// </summary>
    [Serializable]
    public class PistolWeapon : PlayerWeapon
    {
        public void Awake(ref WeaponInformation weaponInformation)
        {
            base.Awake(ref weaponInformation);
        }

        /// <summary>
        /// will be setted to true if a shot is allowed
        /// </summary>
        public override void OnShoot()
        {
            if (DeltaTimeForShot(Time.time))
            {
                _canShoot = true;
            }
        }
        
        public override void OnStopShoot()
        {
            _canShoot = false;
        }

        /// <summary>
        /// shoots a ray from the player
        /// </summary>
        /// <param name="rayDirection"> direction of the ray </param>
        /// <returns> true if a ray was shot and false if not </returns>
        public override bool TryUpdateAndShoot(out Vector3 rayDirection)
        {
            rayDirection = Vector3.zero;
            if (_canShoot && HasAmmoInClip())
            {
                TryHandleRayCastShot(out rayDirection);
                _shootingAudio.Play();
                _canShoot = false;
                return true;
            }

            return false;
        }
    }
}
