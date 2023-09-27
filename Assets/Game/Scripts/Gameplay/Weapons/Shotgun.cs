using System.Collections.Generic;
using Common.ObjectPool;
using Providers;
using UnityEngine;

namespace Weapons
{
    public class Shotgun: Weapon
    {
        private List<Vector2> _spreads = new List<Vector2>()
        {
            new Vector2(-0.15f, -0.1f),
            new Vector2(-0.05f, 0.05f),
            new Vector2(0.15f, 0.1f)
        };
        public override void Fire(Vector3 targetPosition)
        {
            if (IsDelay) return;
            base.Fire(targetPosition);
            for (int i = 0; i < _data.NumberOfShots; i++)
            {
                var bulletPosition = _gunEnd.position + new Vector3(Random.Range(_spreads[i].x, _spreads[i].y), 0.0f, Random.Range(_spreads[i].x, _spreads[i].y));
                var bulletDirection = (bulletPosition - transform.position).normalized;
                var bullet = Pool.Get(PrefabProvider.GetBulletPrefab(Data.BulletType), bulletPosition);
                bullet.Init(bulletDirection, Data.BulletSpeed, Data.Damage, Data.Team);
                _muzzle.Play();
            }
        }
    }
}