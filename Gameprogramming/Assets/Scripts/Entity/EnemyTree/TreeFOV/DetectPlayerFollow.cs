using Entity.BehaviourTree;

namespace Entity.EnemyTree.TreeFOV
{
    /// <summary>
    /// Node to evaluate if the player is in range to follow him.
    /// </summary>
    public class DetectPlayerFollow : Node
    {
        private TreeInfo _info;
        private string rangeOf = "rangeOf";
        private string firstShot = "firstShot";

        public DetectPlayerFollow(ref TreeInfo info)
        {
            _info = info;
        }

        /// <summary>
        /// looks if the player is in range to follow or not
        /// </summary>
        /// <returns> Success if the player is in follow range and failure if not </returns>
        public override NodeState Evaluate()
        {
            if (_info.FieldOfView.IsPlayerInFollowRange())
            {
                _parent.Parent.SetData(rangeOf, InRangeOf.FollowPlayer);
                _nodeState = NodeState.Success;
            }
            else
            {
                _nodeState = NodeState.Failure;
            }

            return _nodeState;
        }
    }
}
