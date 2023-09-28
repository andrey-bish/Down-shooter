using UnityEngine;

namespace Game.ScriptrableObjects.Classes.EffectsData
{
    [CreateAssetMenu(fileName = "EffectData", menuName = "Data/EffectData/EffectData")]
    public class EffectData : ScriptableObject
    {
        [SerializeField] private float _effectValue;

        public float EffectValue => _effectValue;
    }
}