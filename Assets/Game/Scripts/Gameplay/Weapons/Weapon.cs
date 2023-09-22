using Extensions;
using Game.ScriptrableObjects.Classes.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField, GroupComponent] private MeshRenderer _meshRenderer;
        [SerializeField, AssetList, OnValueChanged(nameof(UpdateWeapon)), GroupSetting] protected WeaponData _data;
        public WeaponData Data => _data;

        protected bool _isBoost;
        
        private void Awake()
        {
            UpdateWeapon();
        }

        public virtual void Fire(Vector3 targetPosition)
        {
        }

        private void UpdateWeapon()
        {
            //_meshFilter.sharedMesh = _data.Mesh;
        }

        public virtual void SwitchConstraint(bool value)
        {
        }
        
        public void SwitchShadow(bool value) => _meshRenderer.shadowCastingMode = value ? ShadowCastingMode.On : ShadowCastingMode.Off;

        public void Boosted(bool isBoost) => _isBoost = isBoost;
    }
}