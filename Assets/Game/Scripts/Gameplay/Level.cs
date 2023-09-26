using System;
using UnityEngine;

namespace Gameplay
{
    public class Level : MonoBehaviour
    {
        public event Action OnLevelLoaded;
        public event Action OnLevelCompleted;
        public event Action<int> OnLevelLosing;

        [SerializeField] private LevelController _levelController;

        public bool LevelStarted { get; set; }

        public void StartGameProcess()
        {
            LevelStarted = true;
        }
		
        private void Start()
        {
            OnLevelLoaded?.Invoke();
            _levelController.OnNextLocation += WinLevel;
            _levelController.OnLoseLocation += LoseLevel;
        }

        private void WinLevel()
        {
            LevelStarted = false;
            OnLevelCompleted?.Invoke();
        }

        private void LoseLevel()
        {
            LevelStarted = false;
            OnLevelLosing?.Invoke(_levelController.GetOpenWeapons());
        }

        private void OnDisable()
        {
            _levelController.OnNextLocation -= WinLevel;
            _levelController.OnLoseLocation -= LoseLevel;
        }

#if UNITY_EDITOR
        private void Update()
        {
            // Удачно завершаем уровень
            if (Input.GetKeyDown(KeyCode.Space)) WinLevel();
            // Неудачно завершаем уровень
            if (Input.GetKeyDown(KeyCode.Backspace)) LoseLevel();
        }
#endif

        
    }
}