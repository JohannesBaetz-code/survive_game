using Entity.BehaviourTree;
using UnityEngine;

namespace Entity.EnemyTree.TreeFOV
{
    /// <summary>
    /// Node to check if the player or a bullet is in the near range where the enemy instantly attacks.
    /// </summary>
    public class DetectBulletRange : Node
    {
        private TreeInfo _info;
        private string innerRange = "innerRange";

        public DetectBulletRange(TreeInfo info)
        {
            _info = info;
            _info.EnemyHealthScript.OnHit += OnHit;
        }

        /// <summary>
        /// check if the player or bullet is in near range.
        /// </summary>
        /// <returns> Success if the bullet or player is nearby, failure if not. </returns>
        public override NodeState Evaluate()
        {
            if (_info.FieldOfView.IsDetectingBullets())
            {
                _nodeState = NodeState.Success;
                _parent.Parent.SetData(innerRange, true);
                Debug.Log("Detected Bullet");
            }
            else
            {
                _nodeState = NodeState.Failure;
                _parent.Parent.SetData(innerRange, false);
            }

            return _nodeState;
        }

        private void OnHit()
        {
            _parent.Parent.SetData(innerRange, true);
        }
    }
}
