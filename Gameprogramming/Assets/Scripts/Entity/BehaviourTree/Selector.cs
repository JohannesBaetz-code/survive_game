using System.Collections.Generic;

namespace Entity.BehaviourTree
{
    /// <summary>
    /// special node which runs all child nodes from left to right. If one returns Nodestate.Success this node returns success.
    /// </summary>
    public class Selector : Node
    {
        public Selector() : base() { }

        public Selector(List<Node> children) : base(children) { }

        /// <summary>
        /// evaluates all child nodes. breaks instantly if one node returns success.
        /// </summary>
        /// <returns> Success if one node returns success, Failure if no node returns failure. </returns>
        public override NodeState Evaluate()
        {
            foreach (var node in _children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        continue;
                    case NodeState.Success:
                        _nodeState = NodeState.Success;
                        return _nodeState;
                    case NodeState.Running:
                        _nodeState = NodeState.Running;
                        return _nodeState;
                    default:
                        continue;
                }
            }

            _nodeState = NodeState.Failure;
            return _nodeState;
        }
    }
}