using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.ScriptrableObjects.Classes.EffectsData
{
    [CreateAssetMenu(fileName = "EffectSettingData", menuName = "Data/EffectData/EffectSettingData")]
    public class EffectSettingData : ScriptableObject
    {
        [Group("Heal")]
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)] [SerializeField]
        private EffectData _healData;

        [Group("Heal")] [SerializeField] private float _spawnTimeHealEffect;
        [Group("Heal")][SerializeField] private int _maxHealEffectOnLevel;

        public EffectData HealData => _healData;
        public float SpawnTimeHealEffect => _spawnTimeHealEffect;
        public int MaxHealEffectOnLevel => _maxHealEffectOnLevel;
    }
}