using System;
using System.Collections.Generic;
using Entity.EnemyTree.FOV;
using Health;
using UnityEngine;
using UnityEngine.AI;
using Utility;
using Weapon;

namespace Entity.BehaviourTree
{
    /// <summary>
    /// Tree class to setup a behaviour tree.
    /// </summary>
    [Serializable]
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;
        protected FieldOfView _fow;
        [SerializeField] protected TreeInfo _info;

        public Node Root
        {
            get => _root;
            set => _root = value;
        }

        public FieldOfView FieldOfView
        {
            get => _fow;
            set => _fow = value;
        }

        /// <summary>
        /// get and set all needed references at the beginning of the instantiation
        /// </summary>
        private void Awake()
        {
            _info.ThisEnemy = gameObject;
            _info.WayPointsInHouse = ReferenceTable.WayPointsForEnemies;
            _info.Player = ReferenceTable.Player;
            _info.EnemyHealthScript = GetComponent<Enemy>();
            _info.HealthManager = ReferenceTable.HealthManager;
            _info.FieldOfView = GetComponent<FieldOfView>();
            _info.Agent = GetComponent<NavMeshAgent>();
            Debug.Log("added gameobject to health");
            _info.HealthManager.AddHealthObject(gameObject);
        }

        protected void Start()
        {
            _root = SetupTree();
        }

        /// <summary>
        /// evaluate the tree every frame and decide what the enemy should do.
        /// </summary>
        private void Update()
        {
            if (_root != null)
            {
                _root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
    }

    /// <summary>
    /// additional information and references the nodes need to succeed with their evaluation.
    /// </summary>
    [Serializable]
    public class TreeInfo
    {
        [SerializeField] private List<Transform> _wayPointsInHouse;
        [SerializeField] private EnemyWeapon _enemyWeapon;
        [SerializeField] private float _enemySpeed;
        
        private FieldOfView _fieldOfView;
        private GameObject _player;
        private Enemy _enemyHealthScript;
        private NavMeshAgent _agent;
        private HealthManager _healthManager;

        public bool FirstShot { get; set; }

        public GameObject ThisEnemy { get; set; }

        public List<Transform> WayPointsInHouse
        {
            get => _wayPointsInHouse;
            set => _wayPointsInHouse = value;
        }

        public GameObject Player
        {
            get => _player;
            set => _player = value;
        }

        public float EnemySpeed
        {
            get => _enemySpeed;
            set => _enemySpeed = value;
        }

        public NavMeshAgent Agent
        {
            get => _agent;
            set => _agent = value;
        }

        public FieldOfView FieldOfView
        {
            get => _fieldOfView;
            set => _fieldOfView = value;
        }

        public EnemyWeapon EnemyWeapon
        {
            get => _enemyWeapon;
            set => _enemyWeapon = value;
        }

        public HealthManager HealthManager
        {
            get => _healthManager;
            set => _healthManager = value;
        }

        public Enemy EnemyHealthScript
        {
            get => _enemyHealthScript;
            set => _enemyHealthScript = value;
        }
    }
}
