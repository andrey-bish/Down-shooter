using System;
using DG.Tweening;
using Extensions;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Inventory
{
    public class InventoryCell : MonoBehaviour
    {
        [SerializeField, GroupComponent] private Image _icon;
        [SerializeField, GroupComponent] private Image _lockImage;
        [SerializeField, GroupComponent] private Image _selectCell;

        private Sequence _sequenceRotation;

        private void Start()
        {
            _sequenceRotation = DOTween.Sequence();
        }

        public void SetIconSprite(Sprite sprite)
        {
            _icon.sprite = sprite;
        }

        public void Shake()
        {
            _sequenceRotation.Kill();
            _sequenceRotation = DOTween.Sequence();
            _sequenceRotation.ShakeRotation(_lockImage.transform);
        }

        public void OpenCell()
        {
            _lockImage.transform.DOScale(1.25f, 0.15f).OnComplete(()=>_lockImage.transform.DOScale(0.0f, 0.15f));
        }

        public void Select()
        {
            _selectCell.DOKill();
            _selectCell.DOFade(0.6f, 0.2f);
        }

        public void Unselect()
        {
            _selectCell.DOKill();
            _selectCell.DOFade(0.0f, 0.2f);
        }
    }
}