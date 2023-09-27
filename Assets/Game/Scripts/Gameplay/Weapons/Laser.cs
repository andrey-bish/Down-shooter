using Extensions;
using UnityEngine;

namespace Weapons
{
    public class Laser : MonoBehaviour
    {
        [SerializeField, GroupComponent] private LineRenderer _lineRenderer;
        [SerializeField, GroupComponent] private Transform _startLaserTransform;
        
        [SerializeField, GroupSetting] private float _maxDistanceLaser = 5.0f;
        [SerializeField, GroupSetting] private LayerMask _layerMask;

        private void Update()
        {
            RaycastHit hit;
            _lineRenderer.SetPosition(0, _startLaserTransform.position);
            if (Physics.Raycast(_startLaserTransform.position, _startLaserTransform.forward, out hit,
                    _maxDistanceLaser,_layerMask))
            {
                if (hit.collider)
                {
                    _lineRenderer.SetPosition(1, hit.point);
                }
            }
            else
            {
                _lineRenderer.SetPosition(1, _startLaserTransform.position + _startLaserTransform.forward * _maxDistanceLaser * transform.lossyScale.z);
            }
        }
    }
}