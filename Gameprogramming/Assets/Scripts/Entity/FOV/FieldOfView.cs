using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Entity.EnemyTree.FOV
{

    /// <summary>
    /// class to measure the distance between player and an enemy
    /// </summary>
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] private float _viewRadius;
        [SerializeField] private float _obstacleViewRadius;
        [SerializeField] private float _followRadius;
        [SerializeField] private float _detectBulletRadius;
        [Range(0, 360)]
        [SerializeField] private float _viewAngle;
        [SerializeField] private float _detactionRange;

        [SerializeField] private LayerMask _obstacles;
        [SerializeField] private LayerMask _bullets;
        private GameObject _player;

        public float ViewRadius
        {
            get => _viewRadius;
            set => _viewRadius = value;
        }

        public float ViewAngle
        {
            get => _viewAngle;
            set => _viewAngle = value;
        }

        public float ObstacleViewRadius
        {
            get => _obstacleViewRadius;
            set => _obstacleViewRadius = value;
        }

        public float FollowRadius
        {
            get => _followRadius;
            set => _followRadius = value;
        }

        public float DetectBulletRadius
        {
            get => _detectBulletRadius;
            set => _detectBulletRadius = value;
        }

        private void Awake()
        {
            _player = ReferenceTable.Player;
        }

        public bool IsPlayerInShootRange()
        {
            return PlayerInRangeOf(_viewRadius);
        }

        public bool IsPlayerInFollowRange()
        {
            return PlayerInRangeOf(_followRadius);
        }

        /// <summary>
        /// method to calculate the distance from the enemy to the player
        /// </summary>
        /// <param name="radius"> the max distance the player should habe </param>
        /// <returns> true if the player is in the distance and false if not </returns>
        private bool PlayerInRangeOf(float radius)
        {
            float distanceToPlayer = Vector3.Distance(_player.transform.position, gameObject.transform.position);
            if (distanceToPlayer < _detactionRange + radius)
            {
                Vector3 directionToPlayer = (_player.transform.position - gameObject.transform.position).normalized;
                if (Vector3.Angle(gameObject.transform.forward, directionToPlayer) < _viewAngle / 2)
                {
                    if (!Physics.Raycast(gameObject.transform.position, directionToPlayer, distanceToPlayer, _obstacles))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get the nearest obstacle in Range, so the enemy can use it as cover
        /// </summary>
        /// <param name="nearestObject"> the position of the nearest object </param>
        /// <returns> true if an object is nearby and false if not </returns>
        public bool TryGetObstacleInRange(out Vector3 nearestObject)
        {
            Collider[] obstaclesInRange = Physics.OverlapSphere(gameObject.transform.position, _obstacleViewRadius);

            if (obstaclesInRange == null || obstaclesInRange.Length == 0)
            {
                nearestObject = Vector3.zero;
                return false;
            }

            Vector3 nearest = obstaclesInRange[0].transform.position;
            for (int i = 0; i < obstaclesInRange.Length; i++)
            {
                Vector3 obstacle = obstaclesInRange[i].transform.position;
                float distanceToObstacle = Vector3.Distance(obstacle, gameObject.transform.position);

                if (distanceToObstacle < Vector3.Distance(nearest, gameObject.transform.position))
                {
                    nearest = obstacle;
                }
            }

            nearestObject = nearest;
            return true;
        }

        /// <summary>
        /// Point on circle where the angles in the scene view should be drawn.
        /// </summary>
        /// <param name="angleInDegrees"> angle of the cone of vision </param>
        /// <param name="angleIsGlobal"> if the angle is in local or global space </param>
        /// <returns> the point on the circle </returns>
        public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(MathF.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        /// <summary>
        /// checks if a bullet or the player is nearby the enemy
        /// </summary>
        /// <returns> true if something is nearby or false if not </returns>
        public bool IsDetectingBullets()
        {
            Collider[] bullets = Physics.OverlapSphere(gameObject.transform.position, _detectBulletRadius, _bullets);
            return bullets == null;
        }
    }
}
