using System;
using System.Collections.Generic;
using Entity.Spawner;
using Health;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// class to access certain references everywhere in code for dynamic initialization
    /// </summary>
    public class ReferenceTable : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private HealthManager _healthManager;
        [SerializeField] private Spawner _spawner;

        private List<Transform> _wayPointsForEnemies;
        private static GameObject stPlayer;
        private static HealthManager stHealthManager;
        private static List<Transform> stWayPointsForEnemies;
        private static Spawner stSpawner;

        public static GameObject Player => stPlayer;
        public static HealthManager HealthManager => stHealthManager;
        public static List<Transform> WayPointsForEnemies => stWayPointsForEnemies;
        public static Spawner Spawner => stSpawner;

        private void Awake()
        {
            _wayPointsForEnemies = _spawner.SpawnPoints;
            stPlayer = _player;
            stHealthManager = _healthManager;
            stWayPointsForEnemies = _wayPointsForEnemies;
            stSpawner = _spawner;
        }


    }
}
