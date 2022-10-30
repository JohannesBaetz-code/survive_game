using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Playercontroller.Input;
using Input = Playercontroller.Input.Input;

namespace PlayerController.Movement
{
    /// <summary>
    /// the movement script for the player
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _playerSpeed = 5;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _jumpHeight = 10;
        [SerializeField] private float _runSpeedMultiplier = 1.5f;
        [Range(0, 1)]
        [SerializeField] private float _duckModifier = 0.5f;

        private Input _input;
        private Vector3 _playerVelocity = Vector3.zero;
        private bool _isCrouching;
        private bool _jumped;
        private Vector2 _direction;

        public float PlayerSpeed
        {
            get => _playerSpeed;
            set => _playerSpeed = value;
        }

        private void Awake()
        {
            _input = new Input();
            _input.Enable();
            _isCrouching = false;
            _jumped = false;
            _direction = Vector2.zero;
        }

        /// <summary>
        /// subscribe to all events
        /// </summary>
        private void OnEnable()
        {
            _input.Player.Walk.performed += WalkOnperformed;
            _input.Player.Walk.canceled += WalkOncanceled;
            _input.Player.Jump.performed += JumpOnperformed;
            _input.Player.Run.performed += RunOnperformed;
            _input.Player.Run.canceled += RunOncanceled;
            _input.Player.Crouch.performed += CrouchOnperformed;
            _input.Player.Crouch.canceled += CrouchOncanceled;
        }

        /// <summary>
        /// unsubscribe to all events
        /// </summary>
        private void OnDisable()
        {
            _input.Player.Walk.performed -= WalkOnperformed;
            _input.Player.Walk.canceled -= WalkOncanceled;
            _input.Player.Jump.performed -= JumpOnperformed;
            _input.Player.Run.performed -= RunOnperformed;
            _input.Player.Run.canceled -= RunOncanceled;
            _input.Player.Crouch.performed -= CrouchOnperformed;
            _input.Player.Crouch.canceled -= CrouchOncanceled;
        }

        /// <summary>
        /// Update position every frame
        /// </summary>
        private void Update()
        {
            //on ground check
            if (IsPlayerOnGround() && !_jumped) _playerVelocity.y = 0;

            //player velocity on x and z axis while y axis is cached
            float currentY = _playerVelocity.y;
            _playerVelocity = _direction.x * _camera.transform.right + _direction.y * _camera.transform.forward;
            _playerVelocity *= _playerSpeed;
            _playerVelocity.y = currentY;

            //add gravity and Time.deltaTime
            _playerVelocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_playerVelocity * Time.deltaTime);
            _jumped = false;
        }

        /// <summary>
        /// Walk event and set direction on x and z axis
        /// </summary>
        private void WalkOnperformed(InputAction.CallbackContext obj)
        {
            if (!IsPlayerOnGround()) return;
            _direction = obj.ReadValue<Vector2>();
        }

        private void WalkOncanceled(InputAction.CallbackContext obj)
        {
            if (!IsPlayerOnGround()) return;
            _direction = Vector2.zero;
        }

        /// <summary>
        /// Jump event to change gravity for short time
        /// </summary>
        private void JumpOnperformed(InputAction.CallbackContext obj)
        {
            Debug.Log("Jump" + IsPlayerOnGround());
            if (IsPlayerOnGround())
            {
                _playerVelocity.y += (float)Math.Sqrt(_jumpHeight * -3.0f * _gravity);
                _jumped = true;
            }
        }

        /// <summary>
        /// If run event triggered, add modifier to speed
        /// </summary>
        /// <param name="obj"></param>
        private void RunOnperformed(InputAction.CallbackContext obj)
        {
            if (!IsPlayerOnGround()) return;
            _playerSpeed *= _runSpeedMultiplier;
        }

        private void RunOncanceled(InputAction.CallbackContext obj)
        {
            _playerSpeed /= _runSpeedMultiplier;
        }

        /// <summary>
        /// Scale player for crouch event
        /// </summary>
        /// <param name="obj"></param>
        private void CrouchOnperformed(InputAction.CallbackContext obj)
        {
            if (_isCrouching) return;
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,
                gameObject.transform.localScale.y * _duckModifier, gameObject.transform.localScale.z);
            gameObject.transform.position += Vector3.down;
            _isCrouching = true;
        }

        private void CrouchOncanceled(InputAction.CallbackContext obj)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,
                gameObject.transform.localScale.y / _duckModifier, gameObject.transform.localScale.z);
            _isCrouching = false;
        }

        private bool IsPlayerOnGround() => _characterController.isGrounded;
    }
}
