using System;
using System.Collections.Generic;
using Characters.Player;
using Extensions;
using Game.ScriptrableObjects.Classes.Weapons;
using Sirenix.OdinInspector;
using UI.Inventory;
using UnityEngine;

namespace Gameplay
{
    public class WeaponInventory : MonoBehaviour
    {
        public event Action<int> OnSelectWeapon; 

        [SerializeField, GroupComponent] private WeaponInventoryUI _weaponInventoryUI;
        
        [SerializeField, AssetList] private List<WeaponData> _weaponData;

        private WeaponSelector _weaponSelector;

        private int _numberAvailableWeapons = 0;

        private bool _isGameStarted = true;

        private void Start()
        {
            _weaponInventoryUI.Init(_weaponData);
            _weaponSelector = new();
            _weaponSelector.OnSelectWeapon += SelectWeapon;
        }

        private void OnDestroy()
        {
            _weaponSelector.OnSelectWeapon -= SelectWeapon;
        }

        public void OpenWeapon(int count)
        {
            _numberAvailableWeapons++;
            _weaponInventoryUI.OpenCell(count);
        }

        private void Update()
        {
            if(_isGameStarted) _weaponSelector.CheckSelectWeapon();
        }

        public void StopGame()
        {
            _isGameStarted = false;
            _weaponInventoryUI.Hide();
        }

        private void SelectWeapon(int value)
        {
            if (value > _numberAvailableWeapons)
            {
                _weaponInventoryUI.ShakeCell(value);
            }
            else
            {
                _weaponInventoryUI.SelectCell(value);
                OnSelectWeapon?.Invoke(value);
            }
        }

        public int GetOpenWeapons() => _numberAvailableWeapons;
    }
}