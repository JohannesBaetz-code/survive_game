using Entity.BehaviourTree;

namespace Entity.EnemyTree.EngagePlayer
{
    /// <summary>
    /// Node which stops the player movement.
    /// </summary>
    public class StopGoing : Node
    {
        private TreeInfo _info;
        
        public StopGoing(ref TreeInfo info)
        {
            _info = info;
        }

        /// <summary>
        /// stops the player movement and lets him look at the player
        /// </summary>
        /// <returns></returns>
        public override NodeState Evaluate()
        {
            _info.Agent.destination = _info.ThisEnemy.transform.position;
            _info.ThisEnemy.transform.LookAt(_info.Player.transform.position);
            _nodeState = NodeState.Running;
            return _nodeState;
        }
    }
}