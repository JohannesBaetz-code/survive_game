using Entity.BehaviourTree;

namespace Entity.EnemyTree.EngagePlayer
{
    /// <summary>
    /// Node to let the enemy follow the player
    /// </summary>
    public class FollowPlayer : Node
    {
        private TreeInfo _info;

        public FollowPlayer(TreeInfo info)
        {
            _info = info;
        }

        /// <summary>
        /// sets the direction of the navmesh agent to the player position.
        /// </summary>
        /// <returns> nodestate running </returns>
        public override NodeState Evaluate()
        {
            _info.Agent.destination = _info.Player.transform.position;
            _nodeState = NodeState.Running;
            return _nodeState;
        }
    }
}