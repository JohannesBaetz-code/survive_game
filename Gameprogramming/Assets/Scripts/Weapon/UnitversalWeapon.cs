using System;
using Playercontroller.Shooting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapon
{
    /// <summary>
    /// abstract superclass which all weapons have to inherit
    /// </summary>
    [Serializable]
    public abstract class UniversalWeapon
    {
        [SerializeField] protected int _damage;
        [SerializeField] protected float _range;
        [Range(0, 1), Tooltip("Threshold to lower the random value")]
        [SerializeField] protected float _shootingDistortionOffset;
        [Range(0, 1), Tooltip("Threshold to adjust the occurrence to available random shots")]
        [SerializeField] protected float _shootingDistortingThreshold;
        [Tooltip("Bullets per sec")]
        [SerializeField] protected float _rateOfFire;
        [SerializeField] protected float _reloadTime;
        [SerializeField] protected GameObject _weaponParticle;
        [SerializeField] protected float _particleSpeed;
        [SerializeField] protected GameObject _weaponObject;
        [SerializeField] protected int _maxAmmoClip;
        [SerializeField] protected AudioSource _shootingAudio;
        [SerializeField] protected AudioSource _reloadingAudio;

        protected int _currentAmmoClip;
        protected bool _canShoot;
        protected float _lastShot;

        public GameObject WeaponParticle => _weaponParticle;

        public float ParticleSpeed => _particleSpeed;

        public GameObject WeaponObject
        {
            get => _weaponObject;
            set => _weaponObject = value;
        }

        protected bool HasAmmoInClip() => _currentAmmoClip > 0;

        public float ReloadTime => _reloadTime;
        
        public void Awake()
        {
            _currentAmmoClip = _maxAmmoClip;
            _canShoot = false;
        }

        public abstract void OnShoot();

        public abstract bool TryUpdateAndShoot(out Vector3 transform);

        public abstract void Reload();

        /// <summary>
        /// method to calculate random values for a shot
        /// </summary>
        /// <returns> returns a vector3 with a offset for each stored value </returns>
        protected Vector3 RandomShot()
        {
            if (Random.value < _shootingDistortingThreshold)
            {
                return new Vector3(RandomeDistortionNumber() * _weaponObject.transform.forward.x,
                    RandomeDistortionNumber() * _weaponObject.transform.forward.y,
                    RandomeDistortionNumber() * _weaponObject.transform.forward.z);
            }
            return Vector3.zero;
        }

        /// <summary>
        /// calculates one random number for the random vector3
        /// </summary>
        /// <returns> one random number </returns>
        private float RandomeDistortionNumber()
        {
            float random = Random.value;
            while (random > _shootingDistortionOffset) random -= _shootingDistortionOffset;
            if (Random.value < .5f) random *= -1;
            return random;
        }

        protected bool DeltaTimeForShot(float time) => time - _lastShot >= _rateOfFire;
    }
}
