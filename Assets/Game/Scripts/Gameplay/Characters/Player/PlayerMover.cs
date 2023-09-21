using Extensions;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField, GroupSetting]   private bool          _clampToMaxSpeed = true;
        [SerializeField, GroupComponent] private CharacterBase _character;

        //public bool Active { get; set; } = true;

        private Transform _cameraTransform;
        
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        
        
        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            Vector2 input;
#if UNITY_ANDROID || UNITY_IOS
            var input = TouchInput.Axis;
#else
            input = new Vector2(Input.GetAxisRaw(Horizontal), Input.GetAxisRaw(Vertical));
#endif
            

            var forward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
            var right   = Vector3.ProjectOnPlane(_cameraTransform.right, Vector3.up).normalized;

            var worldInput = forward * input.y + right * input.x;
            var direction  = _clampToMaxSpeed ? worldInput.normalized : Vector3.ClampMagnitude(worldInput, input.magnitude);

            _character.Move(direction);
        }
    }
}