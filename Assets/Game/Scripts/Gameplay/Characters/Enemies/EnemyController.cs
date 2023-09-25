using System;
using System.Collections.Generic;
using Characters.Enemies.Spawner;
using Characters.Enemies.Units;
using Damageable;
using Extensions;
using UnityEngine;

namespace Characters.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        public event Action OnEnemyDie;
        
        [SerializeField, GroupComponent] private List<EnemySpawnPoint> _enemySpawnPoints;
        [SerializeField, GroupComponent] private Transform _playerTransform;
        
        [SerializeField, GroupSetting] private float _startSpawnTime;
        [SerializeField, GroupSetting] private bool _isTurnOffSpawn;

        private List<EnemyBase> _enemies = new ();

        private EnemySpawner _enemySpawner;

        private void Start()
        {
            _enemySpawner = new EnemySpawner(_enemySpawnPoints, _playerTransform, _startSpawnTime, _isTurnOffSpawn);
            _enemySpawner.OnAddedEnemy += AddedEnemy;
            _enemySpawner.StartSpawnEnemies();
        }

        private void AddedEnemy(EnemyBase enemy)
        {
            enemy.OnDie += DieEnemy;
        }

        private void DieEnemy(IDamageable target)
        {
            var enemy = (EnemyBase) target;
            
            if (_enemies.Contains(enemy))
            {
                OnEnemyDie?.Invoke();
                _enemies.Remove(enemy);
                enemy.OnDie -= DieEnemy;
            }
            
            // if (_enemies.Count == 0)
            // {
            //     OnEnemiesEmpty?.Invoke();
            // }
        }
    }
}