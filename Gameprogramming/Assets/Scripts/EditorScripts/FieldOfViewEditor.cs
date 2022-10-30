#if UNITY_EDITOR
using Entity.EnemyTree.FOV;
using UnityEditor;
using UnityEngine;

namespace EditorScripts
{
    /// <summary>
    /// This class has editor scripts to visualize the view radius of an enemy in scene view.
    /// </summary>
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : UnityEditor.Editor
    {
        /// <summary>
        /// This method draws circles around an enemy to show the view radius.
        /// </summary>
        private void OnSceneGUI()
        {
            FieldOfView fow = (FieldOfView) target;

            //red for inner circle where the enemy reacts instant to the player or a bullet passing by
            Handles.color = Color.red;
            Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.DetectBulletRadius);

            //white for the normal radius, where the enemy can detect the player
            Handles.color = Color.white;
            Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.ViewRadius);
            Vector3 viewAngleA = fow.DirectionFromAngle(-fow.ViewAngle / 2, false);
            Vector3 viewAngleB = fow.DirectionFromAngle(fow.ViewAngle / 2, false);

            //these lines are the range in which the enemy can see the player (Sichtkegel)
            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.ViewRadius);
            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.ViewRadius);

            //this is the area, where the enemy will follow the player
            Handles.color = Color.blue;
            Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.FollowRadius);
        }
    }
}
#endif
