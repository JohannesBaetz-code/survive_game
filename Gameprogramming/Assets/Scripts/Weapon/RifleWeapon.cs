using System;
using System.Collections;
using Playercontroller.Shooting;
using UnityEngine;

namespace Weapon
{
    /// <summary>
    /// Implementation of the rifle
    /// </summary>
    [Serializable]
    public class RifleWeapon : PlayerWeapon
    {
        public void Awake(ref WeaponInformation weaponInformation)
        {
            base.Awake(ref weaponInformation);
            Debug.Log(_currentAmmoClip);
        }

        /// <summary>
        /// enables shooting when called
        /// </summary>
        public override void OnShoot()
        {
            _canShoot = true;
            Debug.Log("Rifle Schuss + " + _canShoot);
        }

        /// <summary>
        /// disables shooting
        /// </summary>
        public override void OnStopShoot()
        {
            _canShoot = false;
        }

        /// <summary>
        /// shoots a ray from the camera forward
        /// </summary>
        /// <param name="rayDirection"> direction of the shot ray </param>
        /// <returns> bool if a ray was shot and false if no ray was shot </returns>
        public override bool TryUpdateAndShoot(out Vector3 rayDirection)
        {
            rayDirection = Vector3.zero;
            if (_canShoot && HasAmmoInClip() && DeltaTimeForShot(Time.time))
            {
                TryHandleRayCastShot(out rayDirection);
                _shootingAudio.Play();
                return true;
            }

            return false;
        }
    }
}
