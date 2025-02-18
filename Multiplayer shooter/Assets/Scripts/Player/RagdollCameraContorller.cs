using Game.Input;
using System.Collections;
using UnityEngine;

namespace FabrShooter
{
    [RequireComponent(typeof(RagdollController))]
    public class RagdollCameraContorller : MonoBehaviour
    {
        private const float THIRD_PERSON_CAMERA_X_ROTATION = 25f;

        [SerializeField] private Transform _targetBoneTransform;
        [SerializeField] private Transform _ragdollCameraParent;
        [Space]
        [SerializeField] private Vector3 _ragdollCameraOffset;
        [SerializeField] private Quaternion _rotationOffset;

        private Transform _defaultCameraParent;
        private Transform _cameraTransform;

        private RagdollController _ragdollController;
        private PlayerCameraController _ragdollCameraController;

        private Vector3 _defaultCameraOffset;

        private void Start()
        {
            _cameraTransform = GetComponentInChildren<Camera>().transform;
            _ragdollController = GetComponent<RagdollController>();
            _ragdollCameraController = _ragdollCameraParent.GetComponent<PlayerCameraController>();

            _defaultCameraParent = _cameraTransform.parent;
            _defaultCameraOffset = _cameraTransform.localPosition;

            _ragdollController.OnRagdollEnable += OnRagdollEnable;
            _ragdollController.OnRagdollDisable += OnRagdollDisable;
        }

        private void OnDisable()
        {
            if (_ragdollController == null)
                return;

            _ragdollController.OnRagdollEnable -= OnRagdollEnable;
            _ragdollController.OnRagdollDisable -= OnRagdollDisable;
        }

        private void OnRagdollEnable()
        {
            SetCameraTransform(_ragdollCameraParent, _ragdollCameraOffset, true);
            StartCoroutine(FollowTargetBonePosition());
        }

        private void OnRagdollDisable()
        {
            SetCameraTransform(_defaultCameraParent, _defaultCameraOffset, false);
            StopAllCoroutines();
        }

        private void SetCameraTransform(Transform parent, Vector3 offset, bool enableThirdPersonCameraControll)
        {
            _cameraTransform.parent = parent;
            _cameraTransform.localPosition = offset;

            Quaternion rotation = enableThirdPersonCameraControll ? Quaternion.Euler(THIRD_PERSON_CAMERA_X_ROTATION, 0, 0) : Quaternion.identity;

            _cameraTransform.localRotation = rotation;

            _ragdollCameraController.enabled = enableThirdPersonCameraControll;
        }

        private IEnumerator FollowTargetBonePosition()
        {
            while (true)
            {
                _ragdollCameraParent.transform.position = _targetBoneTransform.position;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}

