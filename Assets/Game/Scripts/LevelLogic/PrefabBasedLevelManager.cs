using System.Collections.Generic;
using Common;
using Game.ScriptrableObjects.Classes;
using Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LevelLogic
{
    public class PrefabBasedLevelManager: LevelManagerBase
    {
        [Header("Settings")]
        [SerializeField, SceneObjectsOnly, Required] private PrefabBasedLevelHolder _levelParent;
        
        [Space]
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [SerializeField] private PrefabsContainer _prefabsContainer;
        
        public List<Level> Levels => _prefabsContainer.Levels;

        public override void LoadLevel()
        {
            UnloadLevel();
            _currentLevelIndex = Statistics.CurrentLevelIndex;


            if (_currentLevelIndex >= _prefabsContainer.Levels.Count)
                _currentLevelIndex = _prefabsContainer.Levels.Count - 1;

            CurrentLevel = Instantiate(_prefabsContainer.Levels[_currentLevelIndex], _levelParent.transform);
            
            CurrentLevel.transform.position = _levelParent.transform.position;
            CurrentLevel.OnLevelLoaded += LevelLoaded;
        }

        public override void LoadLevel(int id)
        {
            UnloadLevel();

            CurrentLevel = Instantiate(_prefabsContainer.Levels[id], _levelParent.transform);
            CurrentLevel.transform.position = _levelParent.transform.position;
            CurrentLevel.OnLevelLoaded += LevelLoaded;
        }

        public override void UnloadLevel(bool editor = false)
        {
            if (CurrentLevel == null) return;

            CurrentLevel.OnLevelCompleted -= CurrentLevel_OnLevelCompleted;
            if (editor)
                DestroyImmediate(CurrentLevel.gameObject);
            else
                Destroy(CurrentLevel.gameObject);
        }

        protected override void IncreaseLevelNumber()
        {
            if (Statistics.AllLevelsCompleted && _levelRandomizerAfterAllLevelsCompleted)
            {
                _currentLevelIndex = RandomLevelNumber.GetAvailable();
            }
            else
            {
                if (_currentLevelIndex <= _prefabsContainer.Levels.Count - 2)
                {
                    _currentLevelIndex++;
                }
                else
                {
                    _currentLevelIndex = _levelRandomizerAfterAllLevelsCompleted ? RandomLevelNumber.GetAvailable() : 0;
                    Statistics.AllLevelsCompleted = true;
                }
            }

            Statistics.CurrentLevelIndex = _currentLevelIndex;
        }
        
        private void LevelLoaded()
        {
            CurrentLevel.OnLevelLoaded -= LevelLoaded;
            CurrentLevel.OnLevelCompleted += CurrentLevel_OnLevelCompleted;
            CurrentLevel.OnLevelLosing += CurrentLevel_OnLevelLosing;

            NotifyOnLevelLoaded();
        }
        
        private void CurrentLevel_OnLevelLosing(int value)
        {
            CurrentLevel.OnLevelCompleted -= CurrentLevel_OnLevelCompleted;
            CurrentLevel.OnLevelLosing -= CurrentLevel_OnLevelLosing;

            NotifyOnLevelNotPassed(value);
        }

        private void CurrentLevel_OnLevelCompleted()
        {
            CurrentLevel.OnLevelCompleted -= CurrentLevel_OnLevelCompleted;
            CurrentLevel.OnLevelLosing -= CurrentLevel_OnLevelLosing;

            IncreaseLevelNumber();
            NotifyOnLevelCompleted();
			
        }

        private void Start() => RandomLevelNumber.Init(_prefabsContainer.Levels.Count);

        private void OnValidate()
        {
            if (_prefabsContainer == null)
            {
                if (PrefabsContainer.Instance == null) Debug.LogError("Не создан PrefabsContainer");

                _prefabsContainer = PrefabsContainer.Instance;
            }

            if (_levelParent == null) _levelParent = FindObjectOfType<PrefabBasedLevelHolder>();
        }
    }
}