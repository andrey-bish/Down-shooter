using System;
using System.Collections.Generic;
using Characters.Enemies.Spawner;
using Characters.Enemies.Units;
using Damageable;
using Extensions;
using Game.ScriptrableObjects.Classes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        public event Action<int> OnEnemyDie;
        
        [SerializeField, GroupComponent] private List<EnemySpawnPoint> _enemySpawnPoints;
        [SerializeField, GroupComponent] private Transform _playerTransform;
        
        [SerializeField, GroupSetting] private float _startSpawnTime;
        [SerializeField, GroupSetting] private bool _isTurnOffSpawn;

        [SerializeField, AssetList] private LevelSettings _levelSettings;

        private List<EnemyBase> _enemies = new ();

        private EnemySpawner _enemySpawner;

        private void Start()
        {
            _levelSettings.SetDefaultValues();
            _enemySpawner = new EnemySpawner(_enemySpawnPoints, _playerTransform, _startSpawnTime, _isTurnOffSpawn, _levelSettings.EnemyTypeSpawn, _levelSettings.SpawnTimeReductionPercentage);
            _enemySpawner.OnAddedEnemy += AddedEnemy;
            _enemySpawner.StartSpawnEnemies();
        }

        private void AddedEnemy(EnemyBase enemy)
        {
            enemy.OnDie += DieEnemy;
            _enemies.Add(enemy);
        }

        private void DieEnemy(IDamageable target)
        {
            var enemy = (EnemyBase) target;
            
            if (_enemies.Contains(enemy))
            {
                OnEnemyDie?.Invoke(enemy.PointsForDeath);
                _enemies.Remove(enemy);
                enemy.OnDie -= DieEnemy;
            }
        }

        public void StopGame()
        {
            _enemySpawner.StopSpawn();
            foreach (var enemy in _enemies)
            {
                enemy.StopMovement();
            }
        }

        public void ChangeEnemySpawnChance() => _enemySpawner.ChangeEnemySpawnChance();
        public void ChangeEnemySpawnTime() => _enemySpawner.ChangeEnemySpawnTime();
    }
}