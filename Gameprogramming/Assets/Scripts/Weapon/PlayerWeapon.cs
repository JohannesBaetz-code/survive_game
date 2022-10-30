using System;
using Playercontroller.Shooting;
using UnityEngine;

namespace Weapon
{
    /// <summary>
    /// abstract class for all weapons the player can use
    /// </summary>
    [Serializable]
    public abstract class PlayerWeapon : UniversalWeapon
    {
        [SerializeField] protected int _maxAmmo;

        protected WeaponInformation _weaponInformation;
        protected int _currentAvailableStoredAmmo;

        protected bool IsStoredAmmoAvailable() => _currentAvailableStoredAmmo > 0;
        protected bool IsAmmoClipFullLoaded() => _currentAmmoClip == _maxAmmoClip;
        public int GetCurrentAmmoClip() => _currentAmmoClip;
        public int GetAvailableAmmo() => _currentAvailableStoredAmmo;
        
        public void Awake(ref WeaponInformation weaponInformation)
        {
            base.Awake();
            _weaponInformation = weaponInformation;
            _currentAvailableStoredAmmo = _maxAmmo;
        }

        public abstract void OnStopShoot();

        /// <summary>
        /// reload the weapon and disable shooting for this time
        /// </summary>
        public override void Reload()
        {
            if (IsStoredAmmoAvailable() && !IsAmmoClipFullLoaded())
            {
                _lastShot = Time.time + _reloadTime;
                int diff = _maxAmmoClip - _currentAmmoClip;
                if (_currentAvailableStoredAmmo > diff)
                {
                    _currentAvailableStoredAmmo -= diff;
                    _currentAmmoClip += diff;
                }
                else
                {
                    _currentAmmoClip += _currentAvailableStoredAmmo;
                    _currentAvailableStoredAmmo = 0;
                }
                _reloadingAudio.Play();
            }
        }

        /// <summary>
        /// shoots raycast from the camera forward with a small random offset to simulate a spray.
        /// </summary>
        /// <param name="rayDirection"> direction of the ray </param>
        protected void TryHandleRayCastShot(out Vector3 rayDirection)
        {
            rayDirection = Vector3.zero;
            if (_currentAmmoClip <= 0) return;

            Vector3 random = RandomShot();
            Ray ray = new Ray();
            ray.direction = _weaponInformation.ShootStartPoint.forward + random;
            ray.origin = _weaponInformation.ShootStartPoint.position + random;

            rayDirection = ray.direction;
            _currentAmmoClip--;
            _lastShot = Time.time;

            if (Physics.Raycast(ray, out RaycastHit hit, _range, _weaponInformation.Layer))
            {
                _weaponInformation.HealthManager.AdjustHealth(hit.collider.gameObject, -_damage);
            }
        }
    }
}
