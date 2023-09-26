using UnityEngine;

namespace UI
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private bool _yOnly;

        private Camera _mainCamera;

        private void Start() => TryGetCamera();

        private void TryGetCamera() => _mainCamera = Camera.main;

        private void LateUpdate()
        {
            if (_mainCamera == default)
            {
                TryGetCamera();
                return;
            }

            transform.eulerAngles = _yOnly
                ? _mainCamera.transform.eulerAngles.y * Vector3.up
                : _mainCamera.transform.eulerAngles;
        }
    }
}