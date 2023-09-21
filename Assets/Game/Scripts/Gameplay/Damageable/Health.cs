using System;
using Extensions;
using UnityEngine;

namespace Damageable
{
    public class Health : MonoBehaviour
    {
        public event Action OnFull;
        public event Action OnEmpty;
        public event Action<float> OnChange;
        public event Action<float> OnInit;

        [GroupView] private float _maxHealth;
        [GroupView] private float _currentHealth;
        public float CurrentHealth => _currentHealth;

        public float Percent => _currentHealth / _maxHealth;
        public bool IsEmpty => _currentHealth <= 0 && _currentHealth < _maxHealth;
        public bool IsFull => _currentHealth >= _maxHealth;

        public void Init(float maxHealth)
        {
            _currentHealth = _maxHealth = maxHealth;
            OnInit?.Invoke(_maxHealth);
        }

        public void Increase(float value)
        {
            _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _maxHealth);
            OnChange?.Invoke(_currentHealth);
            if (!IsFull) return;

            OnFull?.Invoke();
        }

        public void Decrease(float value)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - value, 0, _maxHealth);
            OnChange?.Invoke(_currentHealth);
            if (IsEmpty) OnEmpty?.Invoke();
        }

        public void Empty()
        {
            _currentHealth = 0;
            OnChange?.Invoke(_currentHealth);
            OnEmpty?.Invoke();
        }

        public void Full()
        {
            _currentHealth = _maxHealth;
            //OnChange?.Invoke(_currentHealth);
            OnFull?.Invoke();
        }
    }
}