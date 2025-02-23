using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SunController))]
public class SunControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SunController sunController = (SunController)target;

        GUI.enabled = Application.isPlaying;

        if (GUILayout.Button("Turn off light"))
            sunController.TurnOffSunClientRpc();

        if (GUILayout.Button("Turn on light"))
            sunController.TurnOnSunClientRpc();
    }
}
