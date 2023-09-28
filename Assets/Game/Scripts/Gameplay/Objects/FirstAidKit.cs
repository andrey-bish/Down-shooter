using Characters;
using DG.Tweening;
using Extensions;
using UnityEngine;

namespace Gameplay.Objects
{
    public class FirstAidKit: EffectBase
    {
        [SerializeField, GroupComponent] private Transform _model;
        
        [SerializeField, GroupSetting] private float _rotateDuration;

        public override void ActiveEffect()
        {
            base.ActiveEffect();
            RotateAroundY();
        }

        protected override void InactiveEffect()
        {
            base.InactiveEffect();
            _model.DOKill();
        }

        protected override void ApplyEffect(CharacterBase character)
        {
            base.ApplyEffect(character);
            character.Healing(EffectData.EffectValue);
        }

        private void RotateAroundY()
        {
            _model.DORotate(Vector3.up * 360, _rotateDuration, RotateMode.FastBeyond360).SetRelative().
                SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetLink(gameObject);
            _model.DOLocalMoveY(0.5f, _rotateDuration / 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).
                SetLink(gameObject);
        }
    }
}