using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Random = System.Random;

namespace Entity.Spawner
{

    /// <summary>
    /// Spawns enemywaves.
    /// </summary>
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private float _timeBetweenSpawns;
        [SerializeField] private float _timeBetweenWaves;
        [Tooltip("Time which multiplies every wave on top of TimeBetweenSpawns")]
        [SerializeField] private float _timeModifier;

        [SerializeField] private float _enemiesPerWave;
        [SerializeField] private float _enemiesPerWaveModifier;
        [SerializeField] private GameObject _enemyPrefab;

        private List<GameObject> _enemyWave;
        private Coroutine _spawning;
        private bool _hasFinishedWaveSpawn;

        public List<GameObject> EnemyWave
        {
            get => _enemyWave;
            set => _enemyWave = value;
        }

        public List<Transform> SpawnPoints
        {
            get => _spawnPoints;
            set => _spawnPoints = value;
        }


        private void Awake()
        {
            _enemyWave = new List<GameObject>();
            _hasFinishedWaveSpawn = true;
        }

        /// <summary>
        /// checks if the wave is over or ongoing and if it is finished spawns a new wave after a cooldown
        /// </summary>
        private void Update()
        {
            Debug.Log(_enemyWave.Count);
            if (_enemyWave.Count <= 0)
            {
                if (_hasFinishedWaveSpawn)
                {
                    _spawning = StartCoroutine(SpawnEnemyWave());
                    _hasFinishedWaveSpawn = false;
                }
            }
        }

        /// <summary>
        /// spawns an enemy wave, also modifies time between enemy spawns and time between wave spawns
        /// </summary>
        private IEnumerator SpawnEnemyWave()
        {
            yield return new WaitForSeconds(_timeBetweenWaves);

            for (int i = 0; i < (int)_enemiesPerWave; i++)
            {
                GameObject enemy = Instantiate(_enemyPrefab, _spawnPoints[RandomRoom()].transform.position, Quaternion.identity);
                _enemyWave.Add(enemy);
                yield return new WaitForSeconds(_timeBetweenSpawns);
            }


            _hasFinishedWaveSpawn = true;
            _timeBetweenSpawns *= _timeModifier;
            _enemiesPerWave *= _enemiesPerWaveModifier;
        }

        /// <summary>
        /// calculate a random number to spawn enemies at different places.
        /// </summary>
        /// <returns> the random number for the index of the list of positions </returns>
        private int RandomRoom()
        {
            Random rd = new Random();
            int number = rd.Next(0, _spawnPoints.Count);
            return number;
        }

        public void DeSpawnEnemy(GameObject enemy)
        {
            ReferenceTable.HealthManager.RemoveHealthObject(enemy);
            _enemyWave.Remove(enemy);
            Destroy(enemy);
        }
    }
}
