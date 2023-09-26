using System.Collections.Generic;
using DG.Tweening;
using Extensions;
using Game.ScriptrableObjects.Classes.Weapons;
using UnityEngine;

namespace UI.Inventory
{
    public class WeaponInventoryUI : MonoBehaviour
    {
        [SerializeField, GroupComponent] private List<InventoryCell> _inventoryCells;
        [SerializeField, GroupComponent] private CanvasGroup _canvasGroup;

        private InventoryCell _currentCell;
        
        public void Init(List<WeaponData> weaponData)
        {
            for (var i = 0; i < _inventoryCells.Count; i++)
            {
                _inventoryCells[i].SetIconSprite(weaponData[i].InventorySprite);
            }

            _currentCell = _inventoryCells[0];
            _currentCell.Select();
        }

        public void OpenCell(int value)
        {
            _inventoryCells[value].OpenCell();
        }

        public void ShakeCell(int value)
        {
            _inventoryCells[value].Shake();

        }

        public void SelectCell(int value)
        {
            _currentCell.Unselect();
            _currentCell = _inventoryCells[value];
            _currentCell.Select();
        }

        public void Hide() => _canvasGroup.DOFade(0.0f, 0.25f);
    }
}