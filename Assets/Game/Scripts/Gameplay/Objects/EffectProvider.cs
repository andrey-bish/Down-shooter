using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Game.ScriptrableObjects.Classes.EffectsData;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Gameplay.Objects
{
    public class EffectProvider : MonoBehaviour
    {
        [SerializeField, GroupComponent] private List<EffectBase> _heals;

        [SerializeField, AssetList] private EffectSettingData _effectSettingData;

        private Coroutine _healsRoutine;
        
        public void Init()
        {
            foreach (var heal in _heals)
            {
                heal.transform.localScale = Vector3.zero;
                heal.Deactivate();
            }
            _healsRoutine = StartCoroutine(SpawnHeals());
        }

        public void StopGame()
        {
            _healsRoutine.Stop(this);
        }

        private IEnumerator SpawnHeals()
        {
            while (true)
            {
                yield return Helper.GetWait(_effectSettingData.SpawnTimeHealEffect);
                var countActiveHealEffect = _heals.Count(x => x.gameObject.activeSelf);
                if (countActiveHealEffect < _effectSettingData.MaxHealEffectOnLevel)
                {
                    var inactiveHeals = _heals.Where(x => !x.gameObject.activeSelf).ToList();
                    inactiveHeals[Random.Range(0, inactiveHeals.Count)]
                        .With(x=>x.Activate())
                        .With(x=>x.ActiveEffect());
                }
            }
        }
    }
}