using Entity.BehaviourTree;

namespace Entity.EnemyTree.TreeFOV
{
    /// <summary>
    /// leaf node of DefinePlayerAttackRange. Is doing all the work for evaluating the real distance to the player.
    /// </summary>
    public class DetectPlayerAttackRange : Node
    {
        private TreeInfo _info;
        private bool _inRange;
        private string rangeOf = "rangeOf";

        public DetectPlayerAttackRange(ref TreeInfo info)
        {
            _info = info;
            _inRange = false;
        }

        /// <summary>
        /// looks if the player is in shooting range or isn't
        /// </summary>
        /// <returns> Success if the player is in shooting range, failure if not </returns>
        public override NodeState Evaluate()
        {
            if (_info.FieldOfView.IsPlayerInShootRange())
            {
                _inRange = true;
                _nodeState = NodeState.Success;
                _parent.Parent.Parent.SetData(rangeOf, InRangeOf.Player);
            }
            else
            {
                _inRange = false;
                _nodeState = NodeState.Failure;
            }
            return _nodeState;
        }
    }
}
