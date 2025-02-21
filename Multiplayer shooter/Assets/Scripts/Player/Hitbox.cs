using Unity.Netcode;
using UnityEngine;

namespace FabrShooter
{
    public class Hitbox : NetworkBehaviour
    {
        public Rigidbody Rigidbody { get; private set; }

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();

            if (Rigidbody == null)
                Debug.LogError($"Hitbox script is attached to gameobject without rigidboy", gameObject);
        }
    }
}