using System.Collections;
using System.Collections.Generic;
using Common;
using Common.ObjectPool;
using Game.Scripts.UI;
using UnityEngine;

namespace LevelLogic
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private SceneUI _sceneUI;
        [SerializeField] private LevelManagerBase _levelManager;
        private void Awake() => Application.targetFrameRate = 60;
        
        private void Start()
        {
            _levelManager.OnLevelLoaded += LevelManager_OnLevelLoaded;
            _levelManager.OnLevelCompleted += LevelManager_OnLevelCompleted;
            _levelManager.OnLevelNotPassed += LevelManager_OnLevelNotPassed;
            
            LoadLevel();
        }
        
        private void LoadLevel()
        {
            _levelManager.LoadLevel();
        }

        private void NextLevel()
        {
            Pool.ReleaseAll();
        }
        
        private void RestartGame(bool gameScreen)
        {
            Pool.ReleaseAll();
            LoadLevel();
        }

        private void RestartGameFailedScreen()
        {
            RestartGame(false);
        }

        private void RestartGameInGame()
        {
            if (!_levelManager.CurrentLevel.LevelStarted) return;

            RestartGame(true);
        }


        private void LevelManager_OnLevelLoaded()
        {
           
        }

        private void LevelManager_OnLevelCompleted()
        {
            
        }

        private void LevelManager_OnLevelNotPassed()
        {
            
        }
    }
}

