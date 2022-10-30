using Entity.BehaviourTree;

namespace Entity.EnemyTree.TreeFOV
{
    /// <summary>
    /// Node to get the range to the player and set data according to the player distance
    /// </summary>
    public class DetectPlayer : Node
    {
        private TreeInfo _info;
        private bool _sawPlayer;
        private string foundPlayerString = "FoundPlayer";
        private Node _evaluateDistanceNode;
        private string rangeOf = "rangeOf";

        public DetectPlayer(ref TreeInfo info, Node node)
        {
            _info = info;
            _sawPlayer = false;
            _evaluateDistanceNode = node;
            AttachChildren(node);
            _info.FirstShot = false;
            SetData(rangeOf, InRangeOf.Away);
        }

        /// <summary>
        /// evaluates the given data stored by the different range nodes, sets data for all other nodes at the right place
        /// </summary>
        /// <returns> Success if the player is in attack range and failure if not </returns>
        public override NodeState Evaluate()
        {
            _nodeState = _evaluateDistanceNode.Evaluate();
            InRangeOf currentRangeOf = (InRangeOf) GetData(rangeOf);
            switch (currentRangeOf)
            {
                case InRangeOf.Player:
                    _nodeState = NodeState.Success;
                    _parent.Parent.SetData(rangeOf, InRangeOf.Player);
                    return _nodeState;
                case InRangeOf.Obstacle:
                    _nodeState = NodeState.Failure;
                    _parent.Parent.SetData(rangeOf, InRangeOf.Obstacle);
                    return _nodeState;
                case InRangeOf.FollowPlayer:
                    _nodeState = NodeState.Failure;
                    _parent.Parent.SetData(rangeOf, InRangeOf.FollowPlayer);
                    return _nodeState;
                default:
                    _parent.Parent.SetData(rangeOf, InRangeOf.Away);
                    _nodeState = NodeState.Failure;
                    return _nodeState;
            }
        }
    }

    /// <summary>
    /// ranges the player can have to the enemy
    /// </summary>
    public enum InRangeOf
    {
        Player = 0,
        FollowPlayer = 1,
        Obstacle = 2,
        Away = 3
    }
}
