using FabrShooter.Input;
using System.Collections;
using UnityEngine;

namespace FabrShooter
{
    public class RagdollCameraContorller : MonoBehaviour, IPlayerInitializableComponent
    {
        private const float THIRD_PERSON_CAMERA_X_ROTATION = 25f;

        [SerializeField] private PlayerCamera _ragdollCamera;
        [SerializeField] private RagdollController _ragdollController;
        [SerializeField] private Transform _cameraTransform;
        [Space]
        [SerializeField] private Transform _targetBoneTransform;
        [SerializeField] private Transform _ragdollCameraParent;
        [Space]
        [SerializeField] private Vector3 _ragdollCameraOffset;

        private Transform _defaultCameraParent;
        private PlayerCamera _mainCamera;

        private Vector3 _defaultCameraOffset;

        public void InitializeLocalPlayer()
        {
            _mainCamera = _cameraTransform.GetComponent<PlayerCamera>();

            _defaultCameraParent = _cameraTransform.parent;
            _defaultCameraOffset = _cameraTransform.localPosition;

            _ragdollController.OnRagdollEnable += OnRagdollEnable;
            _ragdollController.OnRagdollDisable += OnRagdollDisable;
        }

        public void InitializeClientPlayer()
        {
            Destroy(this);
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
            if(_mainCamera != null)
                _mainCamera.enabled = !enableThirdPersonCameraControll;
            _ragdollCamera.enabled = enableThirdPersonCameraControll;

            _cameraTransform.parent = parent;
            _cameraTransform.localPosition = offset;

            Quaternion rotation = enableThirdPersonCameraControll ? Quaternion.Euler(THIRD_PERSON_CAMERA_X_ROTATION, 0, 0) : Quaternion.identity;

            _cameraTransform.localRotation = rotation;
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

