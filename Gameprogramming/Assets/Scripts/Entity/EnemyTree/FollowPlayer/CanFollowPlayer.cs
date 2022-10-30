using Entity.BehaviourTree;
using Entity.EnemyTree.TreeFOV;

namespace Entity.EnemyTree.FollowPlayer
{
    /// <summary>
    /// node to check if the enemy can follow the player
    /// </summary>
    public class CanFollowPlayer : Node
    {
        private TreeInfo _info;
        private string rangeOf = "rangeOf";

        public CanFollowPlayer(TreeInfo info)
        {
            _info = info;
        }

        /// <summary>
        /// checks if the player is in follow distance and view distance to the enemy
        /// </summary>
        public override NodeState Evaluate()
        {
            if ((InRangeOf)GetData(rangeOf) == InRangeOf.FollowPlayer)
            {
                _nodeState = NodeState.Success;
                return _nodeState;
            }

            _nodeState = NodeState.Failure;
            return _nodeState;
        }
    }
}
