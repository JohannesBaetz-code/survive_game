using Entity.BehaviourTree;
using Entity.EnemyTree.TreeFOV;
using UnityEngine;

namespace Entity.EnemyTree.EngagePlayer
{
    /// <summary>
    /// Node to attack the player
    /// </summary>
    public class AttackPlayer : Node
    {
        private string _foundPlayerString = "FoundPlayer";
        private EnemyShoot _enemyShoot;
        private TreeInfo _info;
        private string rangeOf = "rangeOf";
        private string innerRange = "innerRange";


        public AttackPlayer(ref TreeInfo info)
        {
            _info = info;
            _enemyShoot = new EnemyShoot(ref info);
        }

        /// <summary>
        /// Evaluate if the player is in attack distance and can be seen
        /// </summary>
        /// <returns> running if the player is attacked and failure if the player cannot be attacked </returns>
        public override NodeState Evaluate()
        {
            //get data from tree
            object rangeOfData = GetData(rangeOf);
            object innerRangeData = GetData(innerRange);

            //if player is not in range return failure
            if (rangeOfData == null)
            {
                _nodeState = NodeState.Failure;
                return _nodeState;
            }

            if (innerRangeData == null)
            {
                _nodeState = NodeState.Failure;
                return _nodeState;
            }

            //if player is in inner range, attack immediately
            if ((bool)innerRangeData)
            {
                _enemyShoot.OnShoot();
                _info.FirstShot = true;
                Debug.Log(_info.FirstShot);
                _enemyShoot.Update();
                ClearData(innerRange);

                _nodeState = NodeState.Running;
                return _nodeState;
            }

            //if player is in outer range attack immediately
            if ((InRangeOf)rangeOfData == InRangeOf.Player)
            {
                _enemyShoot.OnShoot();
                _info.FirstShot = true;
                _enemyShoot.Update();
                ClearData(rangeOf);

                _nodeState = NodeState.Running;
                return _nodeState;
            }

            _nodeState = NodeState.Failure;
            return _nodeState;
        }
    }
}
