using UnityEditor;
using UnityEngine;

namespace FabrShooter.EditorExtension
{
    [CustomEditor(typeof(ComboManager))]
    public class ComboManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ComboManager comboManager = (ComboManager)target;

            GUI.enabled = Application.isPlaying;

            EditorGUILayout.LabelField($"Combo Level: {comboManager.ComboLevel}");
        }
    }

}