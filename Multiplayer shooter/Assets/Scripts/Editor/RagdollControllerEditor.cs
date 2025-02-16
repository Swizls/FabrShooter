using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RagdollController))]
public class RagdollControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RagdollController ragdollController = (RagdollController)target;

        GUI.enabled = Application.isPlaying;

        if (GUILayout.Button("Disable ragdoll"))
            ragdollController.DisableRagdoll();

        if (GUILayout.Button("Enable ragdoll"))
            ragdollController.EnableRagdoll();
    }
}
