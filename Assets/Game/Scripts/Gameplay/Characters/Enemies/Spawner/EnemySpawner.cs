using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.Enemies.Units;
using Common;
using Common.ObjectPool;
using Extensions;
using Game.ScriptrableObjects.Classes;
using Providers;
using UnityEngine;
using Utils;
using static Common.Enums;
using Random = UnityEngine.Random;

namespace Characters.Enemies.Spawner
{
    public class EnemySpawner
    {
        public event Action<EnemyBase> OnAddedEnemy;
        
        private List<EnemySpawnPoint> _enemySpawnPoints;
        private List<LevelSettings.EnemyTypeSpawnChance> _enemyTypeSpawn = new();
        
        private Coroutine _spawnRoutine;
        private WaitForSeconds _wait;
        private RandomNoRepeat _randomNoRepeat = new();
        private Transform _playerTransform;
        private Camera _camera;
        
        private int _countSpawner = 0;
        private int _spawnTimeReductionPercentage;
        
        private float _spawnTime;
        private float _probabilitySumEnemyTypeSpawn;
        
        private bool _isTurnOffSpawn;

        public EnemySpawner(List<EnemySpawnPoint> enemySpawnPoints, Transform playerTransform, float startWaitTime, bool isTurnOffSpawn, List<LevelSettings.EnemyTypeSpawnChance> enemyTypeSpawn, int spawnTimeReductionPercentage)
        {
            _camera = CameraProvider.MainCamera;
            
            _enemySpawnPoints = enemySpawnPoints;
            _randomNoRepeat.Init(_enemySpawnPoints.Count);

            _playerTransform = playerTransform;

            _spawnTime = startWaitTime;
            _spawnTimeReductionPercentage = spawnTimeReductionPercentage;
            _isTurnOffSpawn = isTurnOffSpawn;

            _enemyTypeSpawn = enemyTypeSpawn;
            _probabilitySumEnemyTypeSpawn = _enemyTypeSpawn.Sum(x => x.Probability);
        }
        
        private EnemySpawnPoint GetSpawnPoint() => _enemySpawnPoints[_randomNoRepeat.GetAvailable()];
        
        public void StartSpawnEnemies()
        {
            if (_isTurnOffSpawn) return;
            StopRoutine();
            _spawnRoutine = Coroutines.StartRoutine(SpawnEnemies());
        }

        public void StopSpawn()
        {
            StopRoutine();
        }
        
        private void StopRoutine()
        {
            if (_spawnRoutine != null)
            {
                Coroutines.StopRoutine(_spawnRoutine);
                _spawnRoutine = null;
            }
        }
        
        private IEnumerator SpawnEnemies()
        {
            yield return new WaitForSeconds(0.25f);
            while (true)
            {
                EnemySpawnPoint spawnPoint = null;
                spawnPoint = GetSpawnPoint();
                if (spawnPoint == null) continue;
                yield return null;
                
                //проверка на нахождение спавн поинта в камере 
                var cameraFrustum = GeometryUtility.CalculateFrustumPlanes(_camera);
                if (GeometryUtility.TestPlanesAABB(cameraFrustum, spawnPoint.Bounds)) continue;
                
                _countSpawner++;
                
                var randomEnemyTypeSpawn = GetRandomEnemyTypeSpawnChance();

                var element = PrefabProvider.GetEnemyPrefab(randomEnemyTypeSpawn.EnemyType);
                Pool.Get(element.Prefab, spawnPoint.transform.position)
                    .With(x => x.transform.rotation = spawnPoint.transform.rotation)
                    .With(x => x.Init(element.Data, _playerTransform))
                    .With(x => OnAddedEnemy?.Invoke(x));
                
                yield return Helper.GetWait(_spawnTime);
            }
        }
        
        private LevelSettings.EnemyTypeSpawnChance GetRandomEnemyTypeSpawnChance()
        {
            var probability = Random.Range(0, _probabilitySumEnemyTypeSpawn);

            for (var i = 0; i < _enemyTypeSpawn.Count; i++)
            { 
                probability -= _enemyTypeSpawn[i].Probability;
                if (probability < 0)
                {
                    return _enemyTypeSpawn[i];
                }
            }
            return _enemyTypeSpawn.Last();
        }

        public void ChangeEnemySpawnChance()
        {
            var enemyTypeSpawn = _enemyTypeSpawn.FirstOrDefault(x => x.EnemyType == EnemyType.PistolEnemy);
            if (enemyTypeSpawn != null)
            {
                enemyTypeSpawn.Probability += 0.1f;
                _probabilitySumEnemyTypeSpawn = _enemyTypeSpawn.Sum(x => x.Probability);
            }
        }

        public void ChangeEnemySpawnTime() => _spawnTime -= _spawnTime / _spawnTimeReductionPercentage;
    }
}