using Common.ObjectPool;
using Providers;
using UnityEngine;

namespace Weapons
{
    public class Revolver: Weapon
    {
        private Vector3 _targetPosition;
        
        public override void Fire(Vector3 targetPosition)
        {
            if (IsDelay) return;
            _targetPosition = targetPosition;
            base.Fire(targetPosition);
            SpawnBullet();
        }

        protected override void SpawnBullet()
        {
            var bulletPosition = _gunEnd.position;
            var bulletDirection = (_targetPosition - bulletPosition).normalized;
            var bullet = Pool.Get(PrefabProvider.GetBulletPrefab(Data.BulletType), bulletPosition);
            bullet.Init(bulletDirection, Data.BulletSpeed, Data.Damage, Data.Team);
            _muzzle.Play();
        }
    }
}