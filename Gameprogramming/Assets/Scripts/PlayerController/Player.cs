using System.Collections;
using Health;
using PlayerController.Movement;
using PlayerController.PlayerState;
using UnityEngine;

namespace PlayerController
{
    /// <summary>
    /// player class to handle general behaviour and health
    /// </summary>
    public class Player : MonoBehaviour, IHealth
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _maxStunResistance;
        [SerializeField] private MovementController _movementController;

        private IPlayerState _aliveState = new PlayerAliveState();
        private IPlayerState _deathState = new PlayerDeathState();
        private IPlayerState _currentState;

        private int _currentHealth;
        private int _currentStunResistance;
        private Coroutine _stunTime;

        public IPlayerState AliveState => _aliveState;

        public IPlayerState DeathState => _deathState;

        public int MAXHealth => _maxHealth;

        public int CurrentHealth => _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _currentStunResistance = _maxStunResistance;
            _currentState = _aliveState;
        }

        private void Update()
        {
            _currentState = _currentState.UpdatePlayerState(this);
        }

        public void AdjustHealth(int amount)
        {
            _currentHealth += amount;
        }

        public void StunEntity(float time, float intensityMultiplier)
        {
            _stunTime = StartCoroutine(StunFor(time, intensityMultiplier));
        }

        public void AdjustStunLevel(int amount)
        {
            _currentStunResistance += amount;
        }

        public int GetStunLevel()
        {
            return _currentStunResistance;
        }

        public int GetCurrentHealth()
        {
            return _currentHealth;
        }

        private IEnumerator StunFor(float time, float intensityMultiplier)
        {
            _movementController.PlayerSpeed *= intensityMultiplier;
            yield return new WaitForSeconds(time);
            _movementController.PlayerSpeed /= intensityMultiplier;
        }
    }
}
