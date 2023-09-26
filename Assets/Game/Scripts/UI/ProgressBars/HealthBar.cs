using Damageable;
using Extensions;
using UnityEngine;

namespace UI.ProgressBars
{
    public class HealthBar: ProgressBar
    {
        [SerializeField, GroupSceneObject] private Health _health;

        private void OnEnable()
        {
            _health.OnInit += Init;
            _health.OnChange += Change;
            _health.OnFull += SetMaxHp;
        }

        private void OnDisable()
        {
            _health.OnInit -= Init;
            _health.OnChange -= Change;
            _health.OnFull -= SetMaxHp;
        }

        private void Init(float maxValue)
        {
            SetMaxValue(maxValue, true, true);
            SetValue(maxValue, true, true);
        }

        private void Change(float currentValue)
        {
            SetValue(currentValue);
        }

        private void SetMaxHp()
        {
            ResetHp();
        }
    }
}