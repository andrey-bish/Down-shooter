using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Enemies.Units;
using Common;
using Common.ObjectPool;
using Damageable;
using Extensions;
using Providers;
using UnityEngine;
using static Common.Enums;

namespace Characters.Enemies.Spawner
{
    public class EnemySpawner
    {
        public event Action<EnemyBase> OnAddedEnemy;
        
        private List<EnemySpawnPoint> _enemySpawnPoints;
        
        private Coroutine _spawnRoutine;
        private WaitForSeconds _wait;
        private RandomNoRepeat _randomNoRepeat = new();
        private Transform _playerTransform;

        private int _countSpawner = 0;
        
        private bool _isTurnOffSpawn;

        public EnemySpawner(List<EnemySpawnPoint> enemySpawnPoints, Transform playerTransform, float defaultWaitTime, bool isTurnOffSpawn)
        {
            _enemySpawnPoints = enemySpawnPoints;
            _randomNoRepeat.Init(_enemySpawnPoints.Count);

            _playerTransform = playerTransform;
            
            _wait = new WaitForSeconds(defaultWaitTime);
            _isTurnOffSpawn = isTurnOffSpawn;
        }
        
        private EnemySpawnPoint GetSpawnPoint() => _enemySpawnPoints[_randomNoRepeat.GetAvailable()];
        
        public void StartSpawnEnemies()
        {
            if (_isTurnOffSpawn) return;
            StopRoutine();
            _spawnRoutine = Coroutines.StartRoutine(SpawnEnemies());
        }
        
        private void StopRoutine()
        {
            if (_spawnRoutine != null)
            {
                Coroutines.StopRoutine(_spawnRoutine);
                _spawnRoutine = null;
            }
        }
        
        private void StopSpawn(IDamageable _)
        {
            StopRoutine();
        }

        private IEnumerator SpawnEnemies()
        {
            yield return new WaitForSeconds(0.25f);
            while (true)
            {
                //Debug.Log($"SPAWN  {_countSpawnUnits} >= {MaxUnitsOnWave} {_countWave} => {_enemiesWave.Count} {_enemiesWave[_countWave].Speed}");

                EnemySpawnPoint spawnPoint = null;
                spawnPoint = GetSpawnPoint();
                if (spawnPoint == null) continue;
                
                _countSpawner++;
                
                var element = PrefabProvider.GetEnemyPrefab(EnemyType.SimpleEnemy);
                Pool.Get(element.Prefab, spawnPoint.transform.position)
                    .With(x => x.transform.rotation = spawnPoint.transform.rotation)
                    .With(x => x.Init(element.Data, _playerTransform))
                    .With(x => OnAddedEnemy?.Invoke(x));
                //спавн точка
                
                //точка куда идти
                
                // if (_countSpawnUnits >= MaxUnitsOnWave)
                // {
                //     _countWave++;
                //     if (_countWave >= _enemiesWave.Count)
                //     {
                //         OnEndSpawned?.Invoke();
                //         yield break;
                //     }
                //     _countSpawnUnits = 0;
                // }
                yield return _wait;

            }
        }
    }
}