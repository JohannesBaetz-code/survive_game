using Entity.BehaviourTree;
using UnityEngine;

namespace Entity.EnemyTree.Walk
{
    /// <summary>
    /// Node that lets the enemy patrol between certain points set in the level
    /// </summary>
    public class WalkBetweenPoints : Node
    {
        private TreeInfo _info;

        private int _waypointIndex;
        private float _rangeToDestinationTarget = 4f;
        private float _waitTime = 1f;
        private float _waitedTime;

        public WalkBetweenPoints(ref TreeInfo info)
        {
            _info = info;
            _waypointIndex = 0;
            _waitedTime = 0f;
        }

        /// <summary>
        /// Move Agent to next position in level or let him wait and set new destination
        /// </summary>
        /// <returns></returns>
        public override NodeState Evaluate()
        {
            if (IsAtLocation())
            {
                WaitAndSetNewDestination();
            }
            else
            {
                _info.Agent.destination = _info.WayPointsInHouse[_waypointIndex].position;
            }
            _nodeState = NodeState.Running;
            return _nodeState;
        }

        /// <summary>
        /// checks if the enemy is nearby the next position
        /// </summary>
        /// <returns></returns>
        private bool IsAtLocation()
        {
            return Vector3.Distance(_info.WayPointsInHouse[_waypointIndex].position,
                _info.Agent.gameObject.transform.position) < _rangeToDestinationTarget;
        }

        /// <summary>
        /// lets the enemy wait if he reached the current position and sets him a new direction
        /// </summary>
        private void WaitAndSetNewDestination()
        {
            if (_waitedTime >= _waitTime)
            {
                _waypointIndex = (_waypointIndex + 1) % _info.WayPointsInHouse.Count;
                _info.Agent.destination = _info.WayPointsInHouse[_waypointIndex].position;
                _info.Agent.gameObject.transform.LookAt(_info.WayPointsInHouse[_waypointIndex]);
                _waitedTime = 0f;
            }
            else
            {
                _waitedTime += Time.deltaTime;
            }
        }
    }
}
