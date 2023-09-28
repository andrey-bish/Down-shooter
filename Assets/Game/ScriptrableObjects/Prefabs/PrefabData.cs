using System;
using System.Collections.Generic;
using Characters.Enemies.Units;
using Common.ObjectPool;
using Data.Characters;
using Sirenix.OdinInspector;
using UnityEngine;
using Weapons.Bullets;
using static Common.Enums;

namespace ScriptrableObjects.Prefabs
{
    [CreateAssetMenu(fileName = "PrefabData", menuName = "Data/Prefabs/PrefabData")]
    public class PrefabData: SerializedScriptableObject
    {
        [SerializeField] private Dictionary<ParticleType, PooledParticle> _particlePrefabs;
        [SerializeField] private Dictionary<BulletType, Bullet> _bulletPrefabs;
        
        [SerializeField, TableList] private List<EnemyUnitElement> _enemyUnitPrefabs;
        
        public Dictionary<ParticleType, PooledParticle> ParticlePrefabs => _particlePrefabs;
        public Dictionary<BulletType, Bullet> BulletPrefabs => _bulletPrefabs;
        public List<EnemyUnitElement> EnemyUnitPrefabs => _enemyUnitPrefabs;
    }
    
    [Serializable]
    public class EnemyUnitElement
    {
        [SerializeField] private EnemyType _type;
        [SerializeField] private EnemyBase _prefab;
        [SerializeField] private EnemyData _data;
        public EnemyType Type => _type;
        public EnemyBase Prefab => _prefab;
        public EnemyData Data => _data;
    }
}