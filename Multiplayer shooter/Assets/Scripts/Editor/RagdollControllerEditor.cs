using UnityEditor;
using UnityEngine;

namespace FabrShooter.EditorExtension 
{
    [CustomEditor(typeof(RagdollController))]
    public class RagdollControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            RagdollController ragdollController = (RagdollController)target;

            GUI.enabled = Application.isPlaying;

            if (GUILayout.Button("Disable ragdoll"))
                ragdollController.DisableRagdollClientRpc();

            if (GUILayout.Button("Enable ragdoll"))
                ragdollController.EnableRagdollClientRpc();
        }
    }

}
