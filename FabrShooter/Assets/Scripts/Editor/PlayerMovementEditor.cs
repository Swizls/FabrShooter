using FabrShooter.Player.Movement;
using UnityEditor;
using UnityEngine;

namespace FabrShooter.EditorExtension 
{
    [CustomEditor(typeof(PlayerMovement))]
    public class PlayerMovementEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PlayerMovement playerMovement = (PlayerMovement)target;

            GUI.enabled = Application.isPlaying;

            GUIStyle succesfulStyle = new GUIStyle { normal = { textColor = Color.green } };
            GUIStyle unsuccefulStyle = new GUIStyle { normal = { textColor = Color.red } };

            EditorGUILayout.LabelField($"Current Mover: {playerMovement.CurrentMover.GetType().Name}");
            EditorGUILayout.LabelField($"Velocity: {playerMovement.CharacterController.velocity}");
            EditorGUILayout.LabelField($"Velocity magnitude: {playerMovement.CharacterController.velocity.magnitude}");
            EditorGUILayout.LabelField($"Velocity normalized: {playerMovement.CharacterController.velocity.normalized}");
            EditorGUILayout.LabelField($"Movement Direction: {playerMovement.CurrentMover.MovementDirection}");
            EditorGUILayout.LabelField($"IsMoving: {playerMovement.IsMoving}", playerMovement.IsMoving? succesfulStyle : unsuccefulStyle);
            EditorGUILayout.LabelField($"IsGrounded: {playerMovement.IsGroundend}", playerMovement.IsGroundend ? succesfulStyle : unsuccefulStyle);
            EditorGUILayout.LabelField($"IsFlying: {playerMovement.IsFlying}", playerMovement.IsFlying ? succesfulStyle : unsuccefulStyle);
            EditorGUILayout.LabelField($"IsSliding: {playerMovement.IsSliding}", playerMovement.IsSliding ? succesfulStyle : unsuccefulStyle);
        }
    }

}
