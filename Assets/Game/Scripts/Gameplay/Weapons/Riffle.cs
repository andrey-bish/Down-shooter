using System.Collections;
using Common.ObjectPool;
using Providers;
using UnityEngine;
using Utils;

namespace Weapons
{
    public class Riffle: Weapon
    {
        public override void Fire(Vector3 targetPosition)
        {
            if (_isDelay) return;
            base.Fire(targetPosition);
            StartCoroutine(FireRiffle());
        }

        private IEnumerator FireRiffle()
        {
            for (int i = 0; i < _data.NumberOfShots; i++)
            {
                SpawnBullet();
                yield return Helper.GetWait(0.1f);
            }
        }
    }
}