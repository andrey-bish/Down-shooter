using System;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using static Common.Enums;

namespace Game.ScriptrableObjects.Classes
{
    [CreateAssetMenu(fileName = "LevelSetting", menuName = "Data/LevelSetting")]
    public class LevelSettings : ScriptableObject
    {
        [SerializeField] private List<int> _awardPoints;
        [SerializeField, PreviewField(200, ObjectFieldAlignment.Left)] private List<Sprite> _spritesAward;
        [SerializeField] private List<EnemyTypeSpawnChance> _enemyTypeSpawn;
        [SerializeField, Range(1, 90)] private int _spawnTimeReductionPercentage = 10;
        
        public List<EnemyTypeSpawnChance> EnemyTypeSpawn => _enemyTypeSpawn;
        public List<int> AwardPoints => _awardPoints;
        public List<Sprite> SpritesAward => _spritesAward;
        public int SpawnTimeReductionPercentage => _spawnTimeReductionPercentage;

        public void SetDefaultValues()
        {
            foreach (var spawnChance in _enemyTypeSpawn)
            {
                spawnChance.SetDefaultValue();
            }
        }

        public void ResetValues()
        {
            foreach (var spawnChance in _enemyTypeSpawn)
            {
                spawnChance.ResetValue();
            }
        }

        [Serializable]
        public class EnemyTypeSpawnChance
        {
            [SerializeField, Range(0, 1)] private float _probability;
            [SerializeField] private EnemyType _enemyType;

            private float _defaultProbability = 0;
            
            public float Probability
            {
                get => _probability;
                set => _probability = value;
            }

            public EnemyType EnemyType => _enemyType;

            public void SetDefaultValue()
            {
                if (_defaultProbability != _probability)
                {
                    _defaultProbability = _probability;
                }
            }

            public void ResetValue() => _probability = _defaultProbability;
        }
    }
}