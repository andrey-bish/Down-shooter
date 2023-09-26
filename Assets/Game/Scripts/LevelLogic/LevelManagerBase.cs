using Common;
using Gameplay;
using UnityEngine;

namespace LevelLogic
{
    public abstract class LevelManagerBase : MonoBehaviour
    {
        public event System.Action OnLevelCompleted;
        public event System.Action<int> OnLevelNotPassed;
        public event System.Action OnLevelLoaded;

        public Level CurrentLevel { get; protected set; }

        [SerializeField] protected bool _levelRandomizerAfterAllLevelsCompleted;

        [SerializeField, Sirenix.OdinInspector.ReadOnly] protected int _currentLevelIndex;

        protected readonly RandomNoRepeat RandomLevelNumber = new();

        public abstract void LoadLevel();
        public abstract void LoadLevel(int id);
        public abstract void UnloadLevel(bool editor = false);

        protected abstract void IncreaseLevelNumber();

        protected void NotifyOnLevelCompleted() => OnLevelCompleted?.Invoke();
        protected void NotifyOnLevelNotPassed(int value) => OnLevelNotPassed?.Invoke(value);
        protected void NotifyOnLevelLoaded() => OnLevelLoaded?.Invoke();
    }
}