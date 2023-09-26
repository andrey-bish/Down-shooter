using System;
using Game.Scripts.UI.Windows;
using LevelLogic;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class SceneUI : MonoBehaviour
    {
        public event Action OnTapToPlayClicked;
        public event Action OnRestartInGameClicked;
        public event Action OnRestartOnLoseClicked;
        public event Action OnContinueClicked;
        
        [SerializeField] private GameWindow    _gameWindow;
        [SerializeField] private VictoryWindow _victoryWindow;
        [SerializeField] private LosingWindow  _losingWindow;
        
        private LevelManagerBase _levelManager;

        public GameWindow GameWindow => _gameWindow;
        public LosingWindow LosingWindow => _losingWindow;
        public VictoryWindow VictoryWindow => _victoryWindow;
        
        public void Init(LevelManagerBase levelManager)
		{
			//ActionToPlay.OnComplete += ProcessTapToPlayClicked;
			//_gameWindow.RestartButton.OnClick += ProcessRestartInGameClicked;
			_losingWindow.RestartButton.OnClick += ProcessRestartOnLoseClicked;
			_victoryWindow.ContinueButton.OnClick += ProcessContinueClicked;

			_levelManager = levelManager;
			_levelManager.OnLevelLoaded += ProcessLevelLoaded;
			_levelManager.OnLevelCompleted += ProcessLevelCompleted;
			_levelManager.OnLevelNotPassed += ProcessLevelLose;

			HideAll();
		}

		private void ProcessTapToPlayClicked()
		{
			OnTapToPlayClicked?.Invoke();
		}

		private void ProcessRestartOnLoseClicked() => OnRestartOnLoseClicked?.Invoke();
		private void ProcessContinueClicked() => OnContinueClicked?.Invoke();

		private void HideAll()
		{
			_gameWindow.Enable(false);
			_victoryWindow.Enable(false);
			_losingWindow.Enable(false);
		}

		private void ProcessLevelLoaded()
		{
			_losingWindow.Enable(false);
			_victoryWindow.Enable(false);
			_gameWindow.Enable(true);
		}

		private void ProcessLevelCompleted()
		{
			_gameWindow.Enable(false);
			_victoryWindow.Enable(true);
		}

		private void ProcessLevelLose(int value)
		{
			_gameWindow.Enable(false);
			_losingWindow.SetActiveStars(value);
			_losingWindow.Enable(true);
		}

    }
}