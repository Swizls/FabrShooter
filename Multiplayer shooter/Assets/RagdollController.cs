using Game.Input;
using UnityEngine;
using System.Collections.Generic;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private List<Behaviour> _components;

    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;

    private void Start()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _colliders  = GetComponentsInChildren<Collider>();

        DisableRagdoll();
    }

    public void DisableRagdoll()
    {
        foreach(var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }

        foreach(var component in _components)
        {
            component.enabled = true;
        }
    }

    public void EnableRagdoll()
    {
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }

        foreach (var component in _components)
        {
            component.enabled = false;
        }
    }
}
