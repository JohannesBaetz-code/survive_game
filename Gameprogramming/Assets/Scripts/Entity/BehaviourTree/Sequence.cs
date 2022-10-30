using System.Collections.Generic;

namespace Entity.BehaviourTree
{
    /// <summary>
    /// Special node which only returns success if all nodes return success.
    /// </summary>
    public class Sequence : Node
    {
        public Sequence() : base() { }

        public Sequence(List<Node> children) : base(children) { }

        /// <summary>
        /// returns success if all childs return success. If one child returns failure it breaks evaluation and returns failure. If one child returns running the evaluation returns running.
        /// </summary>
        /// <returns> Success if all childs return success, Failure if one child returns failure, Running if one or more childs return Running </returns>
        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (var node in _children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        _nodeState = NodeState.Failure;
                        return _nodeState;
                    case NodeState.Running:
                        anyChildIsRunning = true;
                        continue;
                    case NodeState.Success:
                        continue;
                    default:
                        _nodeState = NodeState.Success;
                        return _nodeState;
                }
            }
            
            _nodeState = anyChildIsRunning ? NodeState.Running : NodeState.Success;
            return _nodeState;
        }
    }
}