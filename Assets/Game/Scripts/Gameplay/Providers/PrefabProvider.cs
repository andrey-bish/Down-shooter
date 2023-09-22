using System;
using Common.ObjectPool;
using ScriptrableObjects.Prefabs;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Weapons.Bullets;
using static Common.Enums;

namespace Providers
{
    public class PrefabProvider: Singleton<PrefabProvider>
    {
        [SerializeField, AssetList] private PrefabData _prefabData;

        public static PooledParticle GetParticlePrefab(ParticleType type)
        {
            if (!Instance._prefabData.ParticlePrefabs.ContainsKey(type))
            {
                throw new NullReferenceException($"Check Particle Prefab Data {type}");
            }

            return Instance._prefabData.ParticlePrefabs[type];
        }
		
        public static Bullet GetBulletPrefab(BulletType type)
        {
            if (!Instance._prefabData.BulletPrefabs.ContainsKey(type))
            {
                throw new NullReferenceException($"Check Bullet Prefab Data {type}");
            }
            
        
            return Instance._prefabData.BulletPrefabs[type];
        }
    }
}