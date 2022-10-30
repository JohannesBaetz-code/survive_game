using System;
using PlayerController;
using Playercontroller.Shooting;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Input = Playercontroller.Input.Input;

namespace UI
{
    /// <summary>
    /// Script for Ingame UI
    /// </summary>
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Sprite[] _weaponSprites;
        [SerializeField] private Player _player;
        [SerializeField] private ShootController _shootController;

        [SerializeField] private TMP_Text _ammoClipText;
        [SerializeField] private TMP_Text _availableAmmoText;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private GameObject _weaponImage;
        
        private Image _weapon;
        private Input _input;

        private int _ammoClip;
        private int _availableAmmo;
        private int _healthAmount;

        private void Awake()
        {
            _input = new Input();
            _input.Enable();
            _weapon = _weaponImage.GetComponent<Image>();
        }

        private void OnEnable()
        {
            _input.Player.FirstWeapon.performed += FirstWeaponOnperformed;
            _input.Player.SecondWeapon.performed += SecondWeaponOnperformed;
        }

        private void OnDisable()
        {
            _input.Player.FirstWeapon.performed -= FirstWeaponOnperformed;
            _input.Player.SecondWeapon.performed -= SecondWeaponOnperformed;
        }

        private void Start()
        {
            _healthSlider.maxValue = _player.MAXHealth;
            _healthSlider.value = _player.CurrentHealth;
        }

        /// <summary>
        /// update values of ui
        /// </summary>
        private void LateUpdate()
        {
            _ammoClip = _shootController.CurrentWeapon.GetCurrentAmmoClip();
            _availableAmmo = _shootController.CurrentWeapon.GetAvailableAmmo();

            _availableAmmoText.SetText(_availableAmmo.ToString());
            _ammoClipText.SetText(_ammoClip.ToString());
            _healthSlider.value = _player.CurrentHealth;
        }

        /// <summary>
        /// change sprite of weapon to pistol
        /// </summary>
        /// <param name="callbackContext"></param>
        private void FirstWeaponOnperformed(InputAction.CallbackContext callbackContext)
        {
            _weapon.sprite = _weaponSprites[0];
        }

        /// <summary>
        /// change sprite of weapon to rifle
        /// </summary>
        /// <param name="obj"></param>
        private void SecondWeaponOnperformed(InputAction.CallbackContext obj)
        {
            _weapon.sprite = _weaponSprites[1];
        }
    }
}
