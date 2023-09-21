using System;
using UnityEngine;

using static Common.Enums;

namespace Damageable
{
    public interface IDamageable
    {
        public event Action<IDamageable> OnDie;
        public TeamType Team { get; }
        public bool IsDead { get; }
        public Vector3 ModelPosition { get; }
        public Vector3 ShotTargetPosition { get; }
        public void TakeDamage(float value);
        public void TakeDamage(float value, Vector3 hitPosition = default);
    }
}