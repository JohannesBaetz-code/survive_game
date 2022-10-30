using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Health
{
    /// <summary>
    /// Class to handle all health and stuneffects at a central place
    /// </summary>
    public class HealthManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _healthGameObjects;

        private Dictionary<GameObject, IHealth> _healthObjects;
        private Coroutine _decreaseHealthOverTimeCoroutine;
        private InternHealthManager<IHealth> _internHealthManager;

        private void Awake()
        {
            _healthObjects = FindHealthObjects();
            _internHealthManager = new InternHealthManager<IHealth>(this);
        }

        /// <summary>
        /// create new dictionary and add healthObjects to it.
        /// </summary>
        private Dictionary<GameObject, IHealth> FindHealthObjects()
        {
            Dictionary<GameObject, IHealth> healths = new Dictionary<GameObject, IHealth>();
            foreach (var healthGameObject in _healthGameObjects)
            {
                if (healthGameObject.TryGetComponent(out IHealth health))
                {
                    healths.Add(healthGameObject, health);
                }
            }

            return healths;
        }

        /// <summary>
        /// increase or decrease health instant
        /// </summary>
        /// <param name="hitObj"> the object that should be effected </param>
        /// <param name="amount"> the amount to effect it </param>
        public void AdjustHealth(GameObject hitObj, int amount)
        {
            if (TryGetIHealth(out IHealth health, hitObj))
                _internHealthManager.OnObjectHit(health, amount);
        }

        /// <summary>
        /// increase or decrease health over time
        /// </summary>
        /// <param name="hitObj"> the object that should be effected </param>
        /// <param name="amountPerFrame"> the amount to effect it each frame </param>
        /// <param name="frames"> how many frames the entity shall be effected </param>
        /// <param name="maxAmount"> the max value an object can be effected with </param>
        public void AdjustHealth(GameObject hitObj, int amountPerFrame, int frames, int maxAmount = Int32.MaxValue)
        {
            if (TryGetIHealth(out IHealth health, hitObj))
                _internHealthManager.OnObjectHit(health, amountPerFrame, frames, maxAmount);
        }

        /// <summary>
        /// Start Coroutine
        /// </summary>
        /// <param name="healthObject"> the object with health, that should be effected </param>
        /// <param name="amountPerFrame"> the amount every frame shall have an effect </param>
        /// <param name="frames"> the amount of frames that shall have an effect </param>
        /// <param name="maxAmount"> the max amount the effect is allowed to have </param>
        /// <typeparam name="T"> the generic type that inherits from IHealth </typeparam>
        private void StartPrivateCoroutine<T>(T healthObject, int amountPerFrame, int frames, int maxAmount)
            where T : IHealth
            => _decreaseHealthOverTimeCoroutine = StartCoroutine(
                _internHealthManager.DamageOverTime(
                    healthObject,
                    amountPerFrame,
                    frames,
                    maxAmount
                ));

        /// <summary>
        /// Method which tries to get a script which implements IHealth from a gameobject
        /// </summary>
        /// <param name="health"> the script that has IHealth implemented </param>
        /// <param name="hitObj"> the object that has to be tested for health </param>
        /// <returns> true if the object has a script with IHealth implemented, false if not </returns>
        private bool TryGetIHealth(out IHealth health, GameObject hitObj)
        {
            if (_healthObjects.ContainsKey(hitObj))
            {
                health = _healthObjects[hitObj];
                return true;
            }

            health = null;
            return false;
        }

        /// <summary>
        /// Add an object with health to the list
        /// </summary>
        /// <param name="healthObject"></param>
        public void AddHealthObject(GameObject healthObject)
        {
            _healthObjects.Add(healthObject, healthObject.GetComponent<IHealth>());
        }

        public void RemoveHealthObject(GameObject healthObject)
        {
            _healthObjects.Remove(healthObject);
        }

        /// <summary>
        /// private class for dealing with health and stun
        /// </summary>
        /// <typeparam name="T"> generic type to access every script that has implemented IHealth </typeparam>
        private class InternHealthManager<T> where T : IHealth
        {
            private List<T> _takesDamageOverTime;
            private HealthManager _healthManager;

            public InternHealthManager(HealthManager healthManager)
            {
                _healthManager = healthManager;
            }

            /// <summary>
            /// increase or decrease the health of an object
            /// </summary>
            /// <param name="healthObject"> the object with health </param>
            /// <param name="amount"> how much it should be effected </param>
            public void OnObjectHit(T healthObject, int amount)
            {
                healthObject.AdjustHealth(amount);
            }

            /// <summary>
            /// Damage or Heal over time
            /// </summary>
            /// <param name="healthObject"> the object that sould receive damage or heal </param>
            /// <param name="amountPerFrame"> the effect every frame </param>
            /// <param name="frames"> how many frames it shall be effected </param>
            /// <param name="maxAmount"> the max amount of the effect </param>
            public void OnObjectHit(T healthObject, int amountPerFrame, int frames, int maxAmount = int.MaxValue)
            {
                if (_takesDamageOverTime.Contains(healthObject)) return;
                _takesDamageOverTime.Add(healthObject);
                _healthManager.StartPrivateCoroutine(healthObject, amountPerFrame, frames, maxAmount);
            }

            /// <summary>
            /// Coroutine of Damage or Heal over time
            /// </summary>
            public IEnumerator DamageOverTime(T healthObject, int amountPerFrame, int frames, int maxAmount)
            {
                int currentDamage = 0;

                for (int i = 0; i < frames; i++)
                {
                    healthObject.AdjustHealth(amountPerFrame);
                    currentDamage += amountPerFrame;
                    if (currentDamage >= maxAmount) break;
                    yield return null;
                }

                DequeueHealthObject(healthObject);
            }

            private void DequeueHealthObject(T healthObject) => _takesDamageOverTime.Remove(healthObject);
        }
    }
}
