using System;
using System.Collections;
using Entity.EntityState;
using Health;
using UnityEngine;
using Utility;

namespace Entity
{
    /// <summary>
    /// Enemy class to handle the health, State Pattern and VFX of the enemy.
    /// </summary>
    public class Enemy : MonoBehaviour, IHealth
    {
        [SerializeField] private int _health;
        [SerializeField] private GameObject _hitVFX;
        [SerializeField] private float _timeForVFX;
        private GameObject _player;
        private int _currentHealth;
        public Action OnHit;

        private IEntityState _deathState = new EntityDeathState();
        private IEntityState _aliveState = new EntityAliveState();
        private IEntityState _currentState;

        public int CurrentHealth => _currentHealth;

        public IEntityState DeathState => _deathState;

        public IEntityState AliveState => _aliveState;

        public IEntityState CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }

        private void Awake()
        {
            _currentHealth = _health;
            _currentState = _aliveState;
            _player = ReferenceTable.Player;
        }

        /// <summary>
        /// evaluates the state every frame
        /// </summary>
        private void Update()
        {
            _currentState = _currentState.UpdateState(this);
        }

        /// <summary>
        /// increase or decrease health of enemy
        /// </summary>
        /// <param name="amount"></param>
        public void AdjustHealth(int amount)
        {
            _currentHealth += amount;
            OnHit?.Invoke();
            if (amount <= 0)
            {
                GameObject vfx = Instantiate(_hitVFX, gameObject.transform);
                vfx.transform.rotation = Quaternion.Euler(_player.transform.localEulerAngles.x, _player.transform.localEulerAngles.y - 180, _player.transform.localEulerAngles.z);
                Coroutine cr = StartCoroutine(DestroyEffect(vfx));
            }
        }

        /// <summary>
        /// stun entity (not implemented)
        /// </summary>
        /// <param name="time"> time enemy is stunned </param>
        /// <param name="intensityMultiplier"> modifier of stun </param>
        public void StunEntity(float time, float intensityMultiplier)
        {
        }

        /// <summary>
        /// add amount to stundmodifier
        /// </summary>
        /// <param name="amount"></param>
        public void AdjustStunLevel(int amount)
        {
        }

        /// <summary>
        /// get stun modifier (not implemented)
        /// </summary>
        /// <returns> stun modifier </returns>
        public int GetStunLevel()
        {
            return 0;
        }

        public int GetCurrentHealth()
        {
            return _health;
        }

        /// <summary>
        /// triggers the vfx that is triggered when the enemy was hit
        /// </summary>
        /// <param name="effect"> the effect that shall be executed </param>
        private IEnumerator DestroyEffect(GameObject effect)
        {
            yield return new WaitForSeconds(_timeForVFX);
            Destroy(effect);
        }
    }

}
