using System;
using Characters;
using Characters.Player;
using DG.Tweening;
using Extensions;
using Game.ScriptrableObjects.Classes.EffectsData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Objects
{
    public class EffectBase : MonoBehaviour
    {
        [SerializeField, GroupComponent] private ParticleSystem _externalParticle;
        [SerializeField, GroupComponent] private Collider _collider;

        [SerializeField, AssetList] private EffectData _effectData;

        public EffectData EffectData => _effectData;
        
        public virtual void ActiveEffect()
        {
            _collider.enabled = true;
            transform.DOScale(1.0f, 0.25f);
            if (_externalParticle != default) _externalParticle.Play();
        }

        protected virtual void InactiveEffect()
        {
            _collider.enabled = false;
            transform.DOScale(0.0f, 0.25f).OnComplete(() => gameObject.SetActive(false));
            if (_externalParticle != default) _externalParticle.Stop();
        }

        protected virtual void ApplyEffect(CharacterBase character)
        {
            
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent<Player>(out var player))
            {
                ApplyEffect(player);
                InactiveEffect();
            }
        }
    }
}