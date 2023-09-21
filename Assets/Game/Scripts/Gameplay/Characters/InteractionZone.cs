using System;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(SphereCollider))]
    public class InteractionZone : MonoBehaviour
    {
        public event Action<Collider> OnZoneEnter;
        public event Action<Collider> OnZoneExit;

        [SerializeField] private SphereCollider _collider;
        
        public float Radius => _collider.radius;

        public void SwitchCollider()
        {
            _collider.enabled = false;
            _collider.enabled = true;
        }

        public void Init(float radius)
        {
            _collider.radius = radius;
        }

        private void OnTriggerEnter(Collider other)
        {
            OnZoneEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnZoneExit?.Invoke(other);
        }
    }
}