using FabrShooter.Core.SceneManagment;
using UnityEditor;
using UnityEngine;

namespace FabrShooter.EditorExtension 
{
    [CustomEditor(typeof(SceneLoader))]
    public class SceneLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SceneLoader sceneLoader = (SceneLoader)target;

            GUI.enabled = Application.isPlaying;

            if (GUILayout.Button("Load Level"))
                sceneLoader.LoadMainLevel();

            if (GUILayout.Button("Load Menu"))
                sceneLoader.LoadMainMenu();
        }
    }

}
