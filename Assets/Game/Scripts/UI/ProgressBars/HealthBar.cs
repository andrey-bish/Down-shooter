using System;
using Damageable;
using Extensions;
using Providers;
using UnityEngine;

namespace UI.ProgressBars
{
    public class HealthBar: ProgressBar
    {
        [SerializeField, GroupComponent] private Canvas _canvas;
        
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

        private void Start()
        {
            _canvas.worldCamera = CameraProvider.MainCamera;
        }

        private void Init(float maxValue)
        {
            SetMaxValue(maxValue, true, true);
            SetValue(maxValue, true, true);
        }

        private void Change(float currentValue)
        {
            Show(0.25f);
            SetValue(currentValue);
        }

        private void SetMaxHp()
        {
            ResetHp();
        }
    }
}