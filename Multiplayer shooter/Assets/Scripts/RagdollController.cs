using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private GameObject _root;

    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;

    private void Start()
    {
        if (_root == null)
            throw new System.NullReferenceException("Rig root is not setted");

        _rigidbodies = _root.GetComponentsInChildren<Rigidbody>();
        _colliders  = _root.GetComponentsInChildren<Collider>();

        DisableRagdoll();
    }

    public void DisableRagdoll()
    {
        foreach(var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }
    }

    public void EnableRagdoll()
    {
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }
    }
}
