using System;
using System.Collections;
using Health;
using Weapon;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Playercontroller.Shooting
{
    /// <summary>
    /// handles the ability to shoot of the player
    /// </summary>
    public class ShootController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private HealthManager _healthManager;
        [SerializeField] private LayerMask _hitable;

        [SerializeField] private RifleWeapon _rifleWeapon;
        [SerializeField] private PistolWeapon _pistolWeapon;
        [SerializeField] private GameObject _reloadSlider;

        private PlayerWeapon _currentWeapon;

        private Input.Input _input;

        private WeaponInformation _weaponInformation;

        public PlayerWeapon CurrentWeapon
        {
            get => _currentWeapon;
            set => _currentWeapon = value;
        }

        /// <summary>
        /// initialize all weapons and set currentweapon
        /// </summary>
        private void Awake()
        {
            _input = new Input.Input();
            _input.Enable();
            _weaponInformation = new WeaponInformation(_hitable, _camera, _healthManager);
            _pistolWeapon.Awake(ref _weaponInformation);
            _rifleWeapon.Awake(ref _weaponInformation);
            _currentWeapon = _pistolWeapon;
            _currentWeapon.WeaponObject.SetActive(true);
        }

        /// <summary>
        /// subscribe to input events
        /// </summary>
        private void OnEnable()
        {
            _input.Player.Shoot.performed += Shoot;
            _input.Player.Shoot.canceled += StopShoot;
            _input.Player.Reload.performed += Reload;
            _input.Player.FirstWeapon.performed += FirstWeaponOnperformed;
            _input.Player.SecondWeapon.performed += SecondWeaponOnperformed;
        }

        /// <summary>
        /// unsubscribe to input events
        /// </summary>
        private void OnDisable()
        {
            _input.Player.Shoot.performed -= Shoot;
            _input.Player.Shoot.canceled -= StopShoot;
            _input.Player.Reload.performed -= Reload;
            _input.Player.FirstWeapon.performed -= FirstWeaponOnperformed;
            _input.Player.SecondWeapon.performed -= SecondWeaponOnperformed;
        }

        /// <summary>
        /// shoot a bullet and spawn a particle everytime a bullet was spawned
        /// </summary>
        private void Update()
        {
            if (_currentWeapon.TryUpdateAndShoot(out Vector3 rayDirection))
            {
                GameObject particle = Instantiate(_currentWeapon.WeaponParticle);
                particle.transform.position = _currentWeapon.WeaponObject.transform.position;
                particle.GetComponent<Rigidbody>().AddForce(rayDirection.normalized * _currentWeapon.ParticleSpeed, ForceMode.Impulse);
            }
        }

        /// <summary>
        /// shoot with currentweapon
        /// </summary>
        /// <param name="obj"></param>
        private void Shoot(InputAction.CallbackContext obj)
        {
            _currentWeapon.OnShoot();
        }

        /// <summary>
        /// evaluation of event when shooting button was released
        /// </summary>
        /// <param name="obj"></param>
        private void StopShoot(InputAction.CallbackContext obj)
        {
            _currentWeapon.OnStopShoot();
        }

        /// <summary>
        /// reload weapon
        /// </summary>
        /// <param name="obj"></param>
        private void Reload(InputAction.CallbackContext obj)
        {
            _currentWeapon.Reload();
            StartCoroutine(SliderAnimation());
        }

        private IEnumerator SliderAnimation()
        {
            _reloadSlider.transform.localScale = new Vector3(0, 1);
            _reloadSlider.SetActive(true);
            LeanTween.scaleX(_reloadSlider, 2, _currentWeapon.ReloadTime).setEase(LeanTweenType.linear);
            yield return new WaitForSeconds(_currentWeapon.ReloadTime);
            _reloadSlider.SetActive(false);
        }

        /// <summary>
        /// change weapon to second weapon
        /// </summary>
        /// <param name="obj"></param>
        private void SecondWeaponOnperformed(InputAction.CallbackContext obj)
        {
            _currentWeapon.WeaponObject.SetActive(false);
            _currentWeapon = _rifleWeapon;
            _currentWeapon.WeaponObject.SetActive(true);
        }

        /// <summary>
        /// change weapon to first weapon
        /// </summary>
        /// <param name="obj"></param>
        private void FirstWeaponOnperformed(InputAction.CallbackContext obj)
        {
            _currentWeapon.WeaponObject.SetActive(false);
            _currentWeapon = _pistolWeapon;
            _currentWeapon.WeaponObject.SetActive(true);
        }
    }

    /// <summary>
    /// struct to store important weapon information
    /// </summary>
    public struct WeaponInformation
    {
        public LayerMask Layer { get; private set; }
        public Transform ShootStartPoint { get; private set; }
        public HealthManager HealthManager { get; private set; }

        public WeaponInformation(LayerMask layerMask, Camera camera, HealthManager healthManager)
        {
            Layer = layerMask;
            ShootStartPoint = camera.transform;
            HealthManager = healthManager;
        }

        public WeaponInformation(LayerMask layerMask, Transform startPoint, HealthManager healthManager)
        {
            Layer = layerMask;
            ShootStartPoint = startPoint;
            HealthManager = healthManager;
        }
    }
}
