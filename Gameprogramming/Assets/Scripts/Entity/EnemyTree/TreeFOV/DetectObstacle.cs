using Entity.BehaviourTree;
using UnityEngine;

namespace Entity.EnemyTree.TreeFOV
{
    /// <summary>
    /// A node which isn't used because it was used to identify possible locations to get in cover.
    /// </summary>
    public class DetectObstacle : Node
    {
        private TreeInfo _info;
        private string rangeOf = "rangeOf";

        public DetectObstacle(ref TreeInfo info)
        {
            _info = info;
        }

        /// <summary>
        /// checks if a cover is nearby
        /// </summary>
        /// <returns> Success if cover is nearby, failure if not. </returns>
        public override NodeState Evaluate()
        {
            Debug.Log(GetData(rangeOf));
            if (_info.FieldOfView.TryGetObstacleInRange(out Vector3 obstaclePosition))
            {
                SetData("cover", obstaclePosition);
                _parent.Parent.SetData(rangeOf, InRangeOf.Obstacle);
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
