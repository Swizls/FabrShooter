using UnityEngine;

namespace FabrShooter 
{
    [RequireComponent(typeof(Collider))]
    public class WallDetector : MonoBehaviour
    {
        private Collider _trigger;

        public bool IsWallDetected {  get; private set; }

        private void Start()
        {
            _trigger = GetComponent<Collider>();

            _trigger.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            IsWallDetected = true;
        }

        private void OnTriggerExit(Collider other)
        {
            IsWallDetected = false;
        }
    }
}