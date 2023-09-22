using System.Collections.Generic;
using Common.ObjectPool;
using Sirenix.OdinInspector;
using UnityEngine;
using Weapons.Bullets;
using static Common.Enums;

namespace ScriptrableObjects.Prefabs
{
    [CreateAssetMenu(fileName = "PrefabData", menuName = "Prefabs/PrefabData")]
    public class PrefabData: SerializedScriptableObject
    {
        [SerializeField] private Dictionary<ParticleType, PooledParticle> _particlePrefabs;
        [SerializeField] private Dictionary<BulletType, Bullet> _bulletPrefabs;
        
        public Dictionary<ParticleType, PooledParticle> ParticlePrefabs => _particlePrefabs;
        public Dictionary<BulletType, Bullet> BulletPrefabs => _bulletPrefabs;
    }
}