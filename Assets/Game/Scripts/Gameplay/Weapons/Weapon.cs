using Common.ObjectPool;
using Extensions;
using Game.ScriptrableObjects.Classes.Weapons;
using Providers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using static Common.Enums;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField, GroupComponent] private MeshRenderer _meshRenderer;
        [SerializeField, GroupComponent] private Transform _gunEnd;
        [SerializeField, AssetList, OnValueChanged(nameof(UpdateWeapon)), GroupSetting] protected WeaponData _data;
        public WeaponData Data => _data;

        protected bool _isBoost;
        
        private void Awake()
        {
            UpdateWeapon();
        }

        public virtual void Fire(Vector3 targetPosition)
        {
            //targetPosition - 
            var bulletPosition = _gunEnd.position;
            var bulletDirection = (bulletPosition - transform.position).normalized;
            var bullet = Pool.Get(PrefabProvider.GetBulletPrefab(Data.BulletType), bulletPosition);
            bullet.Init(bulletDirection, Data.BulletSpeed, Data.Damage, Data.Team);
            //Pool.Get(PrefabProvider.GetParticlePrefab(ParticleType.PistolFire), _gunEnd.position)
                //.With(x => x.transform.rotation = _gunEnd.rotation);
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