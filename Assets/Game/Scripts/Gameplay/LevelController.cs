using System;
using System.Collections.Generic;
using Characters.Enemies;
using Characters.Player;
using Damageable;
using Extensions;
using Game.ScriptrableObjects.Classes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay
{
    public class LevelController : MonoBehaviour
    {
        public event Action OnNextLocation;
        public event Action OnLoseLocation;

        [SerializeField, GroupComponent] private Player _player;
        [SerializeField, GroupComponent] private EnemyController _enemyController;
        [SerializeField, GroupComponent] private LevelProgress _levelProgress;
        [SerializeField, GroupComponent] private WeaponInventory _weaponInventory;
        
        [SerializeField, AssetList] private LevelSettings _levelSettings;

        private int _currentPoint = 0;
        private int _count = 0;

        private List<int> AwardPoints => _levelSettings.AwardPoints;

        private void Start()
        {
            _player.OnDie += PlayerDie;
            _weaponInventory.OnSelectWeapon += _player.SelectWeapon;
            _enemyController.OnEnemyDie += UpgradeProgress;
            _levelProgress.Init(_levelSettings);
        }
        
        private void OnDestroy()
        {
            _player.OnDie -= PlayerDie;
            _weaponInventory.OnSelectWeapon -= _player.SelectWeapon;
            _enemyController.OnEnemyDie -= UpgradeProgress;
        }
        
        private void PlayerDie(IDamageable player)
        {
            StopGame();
            OnLoseLocation?.Invoke();
        }
        
        private void UpgradeProgress(int value)
        {
            _currentPoint += value;
            _levelProgress.UpgradeProgress(_currentPoint);
            if (_currentPoint >= AwardPoints[_count])
            {
                _count++;
                if (_count >= AwardPoints.Count)
                {
                    OnNextLocation?.Invoke();
                    StopGame();
                    return;
                }
                _weaponInventory.OpenWeapon(_count);
            }
        }

        private void StopGame()
        {
            _enemyController.StopGame();
            _player.StopGame();
            _weaponInventory.StopGame();
            _levelProgress.HideProgressBar();
        }

        public int GetOpenWeapons() => _weaponInventory.GetOpenWeapons();
    }
}