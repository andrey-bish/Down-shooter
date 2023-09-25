using System;
using UnityEngine;

namespace Gameplay
{
    public class Level : MonoBehaviour
    {
        public event Action OnLevelLoaded;
        public event Action OnLevelCompleted;
        public event Action OnLevelLosing;

        //[SerializeField] private LevelController _levelController;

        public bool LevelStarted { get; set; }

        public void StartGameProcess()
        {
            LevelStarted = true;
        }
		
        public void StopGameProcess()
        {
            LevelStarted = false;
        }

        private void Start()
        {
            OnLevelLoaded?.Invoke();
            //_levelController.OnNextLocation += WinLevel;
        }

        private void WinLevel()
        {
            LevelStarted = false;
            OnLevelCompleted?.Invoke();
        }

        public void LoseLevel()
        {
            LevelStarted = false;
            OnLevelLosing?.Invoke();
        }

        private void OnDisable()
        {
            //_levelController.OnNextLocation -= WinLevel;
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