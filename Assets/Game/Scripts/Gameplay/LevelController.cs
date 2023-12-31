﻿using System;
using System.Collections.Generic;
using Characters.Enemies;
using Characters.Player;
using Damageable;
using Extensions;
using Game.ScriptrableObjects.Classes;
using Gameplay.Objects;
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
        [SerializeField, GroupComponent] private EffectProvider _effectProvider;
        
        [SerializeField, AssetList] private LevelSettings _levelSettings;

        private int _currentPoint = 0;
        private int _count = 0;

        private List<int> AwardPoints => _levelSettings.AwardPoints;

        private void Start()
        {
            _player.OnDie += PlayerDie;
            _weaponInventory.OnSelectWeapon += _player.SelectWeapon;
            _enemyController.OnEnemyDie += UpgradeProgress;
            _enemyController.Init(_levelSettings);
            _levelProgress.Init(_levelSettings);
            _effectProvider.Init();
        }
        
        private void OnDestroy()
        {
            _levelSettings.ResetValues();
            _effectProvider.StopGame();
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
                _enemyController.ChangeEnemySpawnChance();
                _enemyController.ChangeEnemySpawnTime();
            }
        }

        private void StopGame()
        {
            _levelSettings.ResetValues();
            _enemyController.StopGame();
            _player.StopGame();
            _weaponInventory.StopGame();
            _levelProgress.HideProgressBar();
            _effectProvider.StopGame();
        }

        public int GetOpenWeapons() => _weaponInventory.GetOpenWeapons();
    }
}