using UnityEngine;

namespace Weapons
{
    public class Pistol: Weapon
    {
        public override void Fire(Vector3 targetPosition)
        {
            if (IsDelay) return;
            base.Fire(targetPosition);
            SpawnBullet();
        }
    }
}