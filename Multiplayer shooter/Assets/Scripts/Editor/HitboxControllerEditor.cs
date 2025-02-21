using UnityEditor;
using UnityEngine;

namespace FabrShooter.EditorExtension 
{
    [CustomEditor(typeof(HitboxController))]
    public class HitboxControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            HitboxController hitboxController = (HitboxController)target;

            GUI.enabled = Application.isPlaying;

            if (GUILayout.Button("Print registred hitbox"))
            {
                if(hitboxController.LastHittedHitbox == null)
                {
                    Debug.Log("Hitbox is not registred yet");
                    return;
                }

                Debug.Log(
                    $"Hitbox name: {hitboxController.LastHittedHitbox.name}; " +
                    $"Owner ID: {hitboxController.LastHittedHitbox.OwnerClientId}; " +
                    $"Network Object ID: {hitboxController.LastHittedHitbox.NetworkObjectId}; " +
                    $"Network Behaviour ID: {hitboxController.NetworkBehaviourId}");
            }
        }
    }

}
