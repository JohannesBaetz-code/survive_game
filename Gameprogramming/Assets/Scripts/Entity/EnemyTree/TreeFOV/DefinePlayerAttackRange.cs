using Entity.BehaviourTree;

namespace Entity.EnemyTree.TreeFOV
{
    /// <summary>
    /// Composite node that checks if the player is in attack range or not. It should have been used for the feature of going in cover, but is not a problem.
    /// </summary>
    public class DefinePlayerAttackRange : Node
    {
        private TreeInfo _info;
        private string inCover = "inCover";
        private string rangeOf = "rangeOf";

        public DefinePlayerAttackRange(ref TreeInfo info, Node node)
        {
            _info = info;
            AttachChildren(node);
            SetData(inCover, true);
        }

        /// <summary>
        /// currently returns the nodestate of the evaluated leaf node. 
        /// </summary>
        /// <returns> the same nodestate as the leaf node. </returns>
        public override NodeState Evaluate()
        {
            _nodeState = _children[0].Evaluate();
            if (_info.FirstShot)
            {
                if ((bool)GetData(inCover))
                {
                    return _nodeState;
                }
                _parent.Parent.ClearData(rangeOf);
                _nodeState = NodeState.Failure;
            }
            return _nodeState;
        }
    }
}
