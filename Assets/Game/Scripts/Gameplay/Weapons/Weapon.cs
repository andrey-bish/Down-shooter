using System.Collections;
using Common.ObjectPool;
using Extensions;
using Game.ScriptrableObjects.Classes.Weapons;
using Providers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using Utils;
using static Common.Enums;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField, GroupComponent] private MeshRenderer _meshRenderer;
        [SerializeField, GroupComponent] protected Transform _gunEnd;
        [SerializeField, AssetList, OnValueChanged(nameof(UpdateWeapon)), GroupSetting] protected WeaponData _data;
        public WeaponData Data => _data;

        protected bool _isBoost;
        [SerializeField] protected bool _isDelay;

        private Coroutine _shotDelayRoutine;
        
        private void Awake()
        {
            UpdateWeapon();
        }

        public virtual void Fire(Vector3 targetPosition)
        {
            _isDelay = true;
            StartCoroutine(ShotDelay());
        }

        private IEnumerator ShotDelay()
        {
            yield return Helper.GetWait(_data.ShotDelay);
            _isDelay = false;
        }

        private void UpdateWeapon()
        {
            //_meshFilter.sharedMesh = _data.Mesh;
        }

        protected virtual void SpawnBullet()
        {
            var bulletPosition = _gunEnd.position;
            var bulletDirection = (bulletPosition - transform.position).normalized;
            var bullet = Pool.Get(PrefabProvider.GetBulletPrefab(Data.BulletType), bulletPosition);
            bullet.Init(bulletDirection, Data.BulletSpeed, Data.Damage, Data.Team);
            //Pool.Get(PrefabProvider.GetParticlePrefab(ParticleType.PistolFire), _gunEnd.position)
            //.With(x => x.transform.rotation = _gunEnd.rotation);
        }

        public virtual void SwitchConstraint(bool value)
        {
        }
        
        public void SwitchShadow(bool value) => _meshRenderer.shadowCastingMode = value ? ShadowCastingMode.On : ShadowCastingMode.Off;

        public void Boosted(bool isBoost) => _isBoost = isBoost;
    }
}